using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTables.AspNet.AspNet5.Extensions.Linq
{
    using Core;

    public class ColumnOptions<T>
    {
        public Expression TargetProperty { get; set; }

        public StringMatchMethod StringMatchMethod { get; set; }

        public bool IgnoreWhenSearching { get; set; }

        public bool IgnoreWhenSorting { get; set; }

        public bool EnableGlobalSearch { get; set; }

        public Expression<Func<T, string, bool>> SearchExpression { get; set; }

        public Expression<Func<T, object>> SortExpression { get; set; }

        public ColumnOptions(Expression targetPropertyExp)
        {
            TargetProperty = targetPropertyExp;
            StringMatchMethod = StringMatchMethod.Contains;
        }
    }
}
