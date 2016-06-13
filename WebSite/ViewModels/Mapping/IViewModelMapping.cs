using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

namespace WebSite.ViewModels.Mapping
{
    interface IViewModelMapping
    {
        void Create(IMapperConfiguration configuration);
    }

    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            foreach (var property in expression.TypeMap.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }

            return expression;
        }
    }
}