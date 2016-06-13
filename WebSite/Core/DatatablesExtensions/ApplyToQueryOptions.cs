using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTables.AspNet.AspNet5.Extensions.Linq
{
    using Core;

    public class ApplyToQueryOptions<T>
    {
        private string currentKey;

        private IDictionary<string, ColumnOptions<T>> options;

        public int? TotalRecords { get; set; }

        public ApplyToQueryOptions(IDataTablesRequest request)
        {
            options = new Dictionary<string, ColumnOptions<T>>();

            InitOptionsFromRequest(request);
        }

        public ColumnOptions<T> GetColumnOptionsFor(string columnName)
        {
            if (!options.ContainsKey(columnName))
                return null;

            return options[columnName];
        }

        public ApplyToQueryOptions<T> ForColumn(string columnName)
        {
            currentKey = columnName;

            return this;
        }

        public ApplyToQueryOptions<T> MapToProperty<TMember>(Expression<Func<T, TMember>> property)
        {
            options[currentKey].TargetProperty = property.Body;

            return this;
        }

        public ApplyToQueryOptions<T> IgnoreWhenSearching()
        {
            options[currentKey].IgnoreWhenSearching = true;

            return this;
        }

        public ApplyToQueryOptions<T> IgnoreWhenSorting()
        {
            options[currentKey].IgnoreWhenSorting = true;

            return this;
        }

        public ApplyToQueryOptions<T> EnableGlobalSearch()
        {
            options[currentKey].EnableGlobalSearch = true;

            return this;
        }

        public ApplyToQueryOptions<T> SearchUsing(Expression<Func<T, string, bool>> expression)
        {
            options[currentKey].SearchExpression = expression;

            return this;
        }

        public ApplyToQueryOptions<T> SortUsing(Expression<Func<T, object>> expression)
        {
            options[currentKey].SortExpression = expression;

            return this;
        }

        /// <summary>
        /// Sets the string match method. Useful only for string properties. Default is String.Contains()
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public ApplyToQueryOptions<T> UseStringMatchMethod(StringMatchMethod method)
        {
            options[currentKey].StringMatchMethod = method;

            return this;
        }

        private void InitOptionsFromRequest(IDataTablesRequest request)
        {
            var applyToType = typeof(T);

            foreach (var column in request.Columns.Where(c => c.IsSearchable || c.IsSortable))
            {
                //Try to match property automatically by column.Field
                var prop = applyToType.GetProperty(column.Field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                //Try to match property automatically by column.Name
                if (prop == null)
                    prop = applyToType.GetProperty(column.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                //No match, set TargetProperty to null
                if (prop == null)
                {
                    options.Add(column.Name, new ColumnOptions<T>(null));
                    continue;
                }

                //Property is found, create accessing expression
                var propertyExp = Expression.MakeMemberAccess(Expression.Parameter(typeof(T), "target"), prop);

                options.Add(column.Name, new ColumnOptions<T>(propertyExp));
            }
        }
    }
}
