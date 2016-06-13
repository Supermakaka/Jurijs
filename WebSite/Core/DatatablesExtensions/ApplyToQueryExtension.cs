using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace DataTables.AspNet.AspNet5.Extensions.Linq
{
    using Core;

    public static class ApplyToQueryExtension
    {
        private static HashSet<Type> IntegralTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(short),
            typeof(int),
            typeof(long)
        };

        public static bool ColumnHasSearchValue(this IDataTablesRequest request, string columnName)
        {
            return !String.IsNullOrEmpty(GetColumnSearchValue(request, columnName));
        }

        public static string GetColumnSearchValue(this IDataTablesRequest request, string columnName)
        {
            var col = request.Columns.FirstOrDefault(c => c.Name == columnName && c.IsSearchable && c.Search != null);

            if (col == null)
                return String.Empty;

            return col.Search.Value;
        }

        public static int? GetColumnSearchValueAsInt(this IDataTablesRequest request, string columnName)
        {
            if (!ColumnHasSearchValue(request, columnName))
                return null;

            return int.Parse(GetColumnSearchValue(request, columnName));
        }

        public static bool? GetColumnSearchValueAsBoolean(this IDataTablesRequest request, string columnName)
        {
            if (!ColumnHasSearchValue(request, columnName))
                return null;

            return bool.Parse(GetColumnSearchValue(request, columnName));
        }

        public static FilteringResult<T> ApplyToQuery<T>(this IDataTablesRequest request, IQueryable<T> query)
        {
            return ApplyToQuery<T>(request, query, null);
        }

        public static FilteringResult<T> ApplyToQuery<T>(this IDataTablesRequest request, IQueryable<T> query, Expression<Func<ApplyToQueryOptions<T>, ApplyToQueryOptions<T>>> options)
        {
            var result = new FilteringResult<T>();

            var opt = new ApplyToQueryOptions<T>(request);

            if (options != null)
                options.Compile()(opt);

            VerifyOptionsAgainstRequest<T>(request, opt);
            
            var q = query;

            result.TotalRecords = opt.TotalRecords.HasValue ? opt.TotalRecords.Value : q.Count();

            //apply searching
            q = ApplySearchExpression(q, request, opt);

            //apply sorting
            q = ApplySortExpression(q, request, opt);

            result.TotalRecordsFiltered = q.Count();

            //apply pagination
            if (request.Length > 0)
                q = q.Skip(request.Start).Take(request.Length);

            result.QueryFiltered = q.Cast<T>();

            return result;
        }

        private static void VerifyOptionsAgainstRequest<T>(IDataTablesRequest request, ApplyToQueryOptions<T> options)
        {
            var searchableColumns = request.Columns.Where(c => c.IsSearchable);

            foreach (var col in searchableColumns)
            {
                var opt = options.GetColumnOptionsFor(col.Name);

                if (!opt.IgnoreWhenSearching && opt.TargetProperty == null && opt.SearchExpression == null)
                    throw new Exception(String.Format("Can't find the target property for searchable column '{0}' in type '{1}'. Mark column as not searchable or use MapToProperty(), UseSearchExpression() or IgnoreWhenSearching() methods.", col.Name, typeof(T)));

                if (opt.SearchExpression != null)
                {
                    bool parameterIsUsed = ExpressionHelper.FindParameter(opt.SearchExpression.Body, opt.SearchExpression.Parameters[1]);

                    if (!parameterIsUsed)
                        throw new Exception(String.Format("Search value is not used in SearchExpression for column '{0}'", col.Name));
                }
            }

            var sortableColumns = request.Columns.Where(c => c.IsSortable);

            foreach (var col in sortableColumns)
            {
                var opt = options.GetColumnOptionsFor(col.Name);

                if (!opt.IgnoreWhenSorting && opt.TargetProperty == null && opt.SortExpression == null)
                    throw new Exception(String.Format("Can't find the target property for sortable column '{0}' in type '{1}'. Mark column as not searchable or use MapToProperty(), UseSortExpression() or IgnoreWhenSorting() methods.", col.Name, typeof(T)));
            }
        }

        private static IQueryable<T> ApplySearchExpression<T>(IQueryable<T> query, IDataTablesRequest request, ApplyToQueryOptions<T> options)
        {
            ParameterExpression target = Expression.Parameter(typeof(T), "target");

            var columnSearchExp = BuildColumnSearchExpression(query, request, options, target);
            var globalSearchExp = BuildGlobalSearchExpression(query, request, options, target);

            if (columnSearchExp == null && globalSearchExp == null)
                return query;

            Expression searchExp = (columnSearchExp != null) ? columnSearchExp : globalSearchExp;

            if (columnSearchExp != null && globalSearchExp != null)
                searchExp = Expression.AndAlso(columnSearchExp, globalSearchExp);

            return ExpressionHelper.AddWhere(query, searchExp, target);
        }

        private static Expression BuildColumnSearchExpression<T>(IQueryable<T> query, IDataTablesRequest request, ApplyToQueryOptions<T> options, ParameterExpression target)
        {
            var filteredColumns = request.Columns.Where(c => c.IsSearchable && !String.IsNullOrEmpty(c.Search.Value));

            if (filteredColumns.Count() == 0)
                return null;

            Expression exp = null;

            foreach (var col in filteredColumns)
            {
                var opt = options.GetColumnOptionsFor(col.Name);

                if (opt.IgnoreWhenSearching)
                    continue;

                if (String.IsNullOrEmpty(col.Search.Value))
                    return null;

                Expression matchExp = null;

                if (opt.SearchExpression != null)
                {
                    //replace expression parameters
                    matchExp = ExpressionHelper.Replace(opt.SearchExpression.Body, opt.SearchExpression.Parameters[0], target);
                    matchExp = ExpressionHelper.Replace(matchExp, opt.SearchExpression.Parameters[1], Expression.Constant(col.Search.Value));
                }
                else
                {
                    matchExp = BuildMatchExpression(opt.TargetProperty, col.Search.Value, opt.StringMatchMethod, target);
                }

                if (matchExp != null)
                    exp = (exp == null) ? matchExp : Expression.AndAlso(exp, matchExp);
            }

            return exp;
        }

        private static Expression BuildGlobalSearchExpression<T>(IQueryable<T> query, IDataTablesRequest request, ApplyToQueryOptions<T> options, ParameterExpression target)
        {
            if (String.IsNullOrEmpty(request.Search.Value))
                return null;

            Expression exp = null;

            foreach (var col in request.Columns.Where(c => c.IsSearchable))
            {
                var opt = options.GetColumnOptionsFor(col.Name);

                if (opt.IgnoreWhenSearching)
                    continue;

                Expression matchExp = null;

                if (!opt.EnableGlobalSearch)
                {
                    matchExp = Expression.Constant(false);
                }
                else if (opt.SearchExpression != null)
                {
                    //replace expression parameters
                    matchExp = ExpressionHelper.Replace(opt.SearchExpression.Body, opt.SearchExpression.Parameters[0], target);
                    matchExp = ExpressionHelper.Replace(matchExp, opt.SearchExpression.Parameters[1], Expression.Constant(request.Search.Value));
                }
                else
                {
                    matchExp = BuildMatchExpression(opt.TargetProperty, request.Search.Value, opt.StringMatchMethod, target);
                }

                if (matchExp != null)
                    exp = (exp == null) ? matchExp : Expression.OrElse(exp, matchExp);
            }

            return exp;
        }

        private static Expression BuildMatchExpression(Expression targetProperty, string propertyValue, StringMatchMethod matchMethod, ParameterExpression target)
        {
            if (targetProperty == null)
                return null;

            if (String.IsNullOrEmpty(propertyValue))
                return null;

            Expression noMatch = Expression.Constant(false);

            var propertyExp = ExpressionHelper.ExtractPropertyChain(targetProperty, target);

            if (IsIntegral(propertyExp.Type))
            {
                int val;
                if (!int.TryParse(propertyValue, out val))
                    return noMatch;

                return Expression.Equal(propertyExp, Expression.Constant(val));
            }

            if (IsBoolean(propertyExp.Type))
            {
                bool val;
                if (!bool.TryParse(propertyValue, out val))
                    return noMatch;

                return Expression.Equal(propertyExp, Expression.Constant(val));
            }

            if (IsEnum(propertyExp.Type))
            {
                var val = Enum.Parse(propertyExp.Type, propertyValue);

                if (val == null)
                    return noMatch;
                
                var valExp = Expression.Constant(val, propertyExp.Type);

                return Expression.Equal(Expression.Convert(propertyExp, Enum.GetUnderlyingType(propertyExp.Type)), Expression.Convert(valExp, Enum.GetUnderlyingType(propertyExp.Type)));
            }

            if (IsDateTime(propertyExp.Type))
            {
                var range = propertyValue.Split('-');

                if (range == null)
                    return noMatch;

                var propertyExpAsNullable = Expression.Convert(propertyExp, typeof(Nullable<>).MakeGenericType(propertyExp.Type));

                if (range.Length > 1)
                {
                    DateTime dateFrom, dateTill;

                    if (!DateTime.TryParse(range[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateFrom))
                        return noMatch;

                    if (!DateTime.TryParse(range[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTill))
                        return noMatch;

                    dateTill = dateTill.AddDays(1);

                    var propertyGreatherThan = Expression.GreaterThanOrEqual(propertyExpAsNullable, Expression.Constant(dateFrom, typeof(DateTime?)));
                    var propertyLessThan = Expression.LessThan(propertyExpAsNullable, Expression.Constant(dateTill, typeof(DateTime?)));

                    return Expression.AndAlso(propertyGreatherThan, propertyLessThan);
                }
                else
                {
                    DateTime date;

                    if (!DateTime.TryParse(propertyValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        return noMatch;

                    //if needed, it is possible to match by date part only, utilizing the DbFunctions.TruncateTime method (EF6).
                    //MethodInfo methodInfo = typeof(DbFunctions).GetMethod("TruncateTime", new Type[] { propertyExpAsNullable.Type });
                    //return Expression.Equal(Expression.Call(methodInfo, propertyExpAsNullable), Expression.Constant(date, typeof(DateTime?)));

                    return Expression.Equal(propertyExpAsNullable, Expression.Constant(date, typeof(DateTime?)));
                }
            }

            var propertyExpAsString = Expression.Call(Expression.Convert(propertyExp, typeof(object)), typeof(object).GetMethod("ToString"));

            var propertyContainsExpr = Expression.Call(propertyExpAsString, typeof(string).GetMethod(matchMethod.ToString(), new[] { typeof(string) }), Expression.Constant(propertyValue));

            //if the type is nullable, add additional check to be not null
            if (Nullable.GetUnderlyingType(propertyExp.Type) != null)
            {
                var propertyNotNullExpr = Expression.NotEqual(propertyExp, Expression.Constant(null, propertyExp.Type));

                return Expression.AndAlso(propertyNotNullExpr, propertyContainsExpr);
            }

            return propertyContainsExpr;
        }

        private static IQueryable<T> ApplySortExpression<T>(IQueryable<T> query, IDataTablesRequest request, ApplyToQueryOptions<T> options)
        {
            var sortedColumns = request.Columns.Where(c => c.IsSortable && c.Sort != null).OrderBy(c => c.Sort.Order);

            if (sortedColumns.Count() == 0)
                return query;

            var q = query;

            ParameterExpression target = Expression.Parameter(typeof(T), "target");

            int cnt = 0;

            foreach (var col in sortedColumns)
            {
                var opt = options.GetColumnOptionsFor(col.Name);

                if (opt.IgnoreWhenSorting)
                    continue;

                Expression sortExp = null;

                if (opt.SortExpression != null)
                {
                    //replace expression parameters
                    sortExp = ExpressionHelper.Replace(opt.SortExpression.Body, opt.SortExpression.Parameters[0], target);
                }
                else if (opt.TargetProperty != null)
                {
                    sortExp = ExpressionHelper.ExtractPropertyChain(opt.TargetProperty, target);
                }

                if (sortExp != null)
                {
                    if (cnt == 0)
                        q = (col.Sort.Direction == SortDirection.Ascending) ? ExpressionHelper.AddOrderBy(q, sortExp, target) : ExpressionHelper.AddOrderByDescending(q, sortExp, target);
                    else
                        q = (col.Sort.Direction == SortDirection.Ascending) ? ExpressionHelper.AddThenBy(q, sortExp, target) : ExpressionHelper.AddThenByDescending(q, sortExp, target);

                    cnt++;
                }
            }

            return q;
        }

        private static bool IsIntegral(Type type)
        {
            return IntegralTypes.Contains(type) || IntegralTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static bool IsBoolean(Type type)
        {
            return type == typeof(bool) || Nullable.GetUnderlyingType(type) == typeof(bool);
        }

        private static bool IsDateTime(Type type)
        {
            return type == typeof(DateTime) || Nullable.GetUnderlyingType(type) == typeof(DateTime);
        }

        private static bool IsEnum(Type type)
        {
            if (type.IsEnum)
                return true;

            var underlyingType = Nullable.GetUnderlyingType(type);

            if (underlyingType != null)
                return underlyingType.IsEnum;

            return false;
        }
    }
}