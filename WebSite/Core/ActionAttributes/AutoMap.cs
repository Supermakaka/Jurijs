using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoMapper;

namespace WebSite
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        public Type SourceType { get; private set; }

        public Type DestType{ get; private set; }

        public AutoMapAttribute(Type sourceType, Type destType)
        {
            SourceType = sourceType;
            DestType = destType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;

            object viewModel = DependencyResolver.Current.GetService<IMapper>().Map(model, SourceType, DestType);

            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}