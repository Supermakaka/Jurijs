using AutoMapper.Mappers;
using AutoMapper;
using BusinessLogic.IdentityStores;
using BusinessLogic.Models;
using BusinessLogic.Services;
using MessageServices.Identity;
using MessageServices;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.Web;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web;
using System;
using WebSite.Core.Identity;
using WebSite.ViewModels.Mapping;

namespace WebSite
{
    public static class SimpleInjectorConfig
    {
        public static void RegisterDependencyInjection()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            //Asp.Net Identity
            container.Register<IdentitySignInManager>(Lifestyle.Scoped);
            container.Register<IdentityUserManager>(Lifestyle.Scoped);
            container.Register<IdentityRoleManager>(Lifestyle.Scoped);
            container.Register<IIdentitySmsService, SmsService>(Lifestyle.Scoped);

            //Asp.Net Identity internal dependencies
            container.Register(() =>
                AdvancedExtensions.IsVerifying(container)
                ? new OwinContext(new Dictionary<string, object>()).Authentication
                : HttpContext.Current.GetOwinContext().Authentication,
                Lifestyle.Scoped);
            container.Register<IUserStore<User, int>, UserStore>(Lifestyle.Scoped);
            container.Register<IRoleStore<Role, int>, RoleStore>(Lifestyle.Scoped);

            //This is how dependency injection is setup when multiple interfaces are inhereted by the same class.
            var emailServiceRegistration = Lifestyle.Scoped.CreateRegistration(typeof(EmailService), container);
            container.AddRegistration(typeof(IIdentityEmailService), emailServiceRegistration);
            container.AddRegistration(typeof(IEmailService), emailServiceRegistration);

            //DataContext
            var dataContextRegistration = Lifestyle.Scoped.CreateRegistration(typeof(DataContext), container);
            container.AddRegistration(typeof(DataContext), dataContextRegistration);
            container.AddRegistration(typeof(IDataContext), dataContextRegistration);

            RegisterServices(container);
            RegisterAutomapper(container);

            //AutoMapper Resolvers
            container.Register<UserRoleListToStringResolver>(Lifestyle.Scoped);

            //Integrated
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        public static void RegisterAutomapper(Container container)
        {
            var projectMappings = typeof(IViewModelMapping).Assembly.GetExportedTypes()
                .Where(x => !x.IsAbstract && typeof(IViewModelMapping).IsAssignableFrom(x))
                .Select(Activator.CreateInstance)
                .Cast<IViewModelMapping>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(type => container.GetInstance(type));

                foreach (var m in projectMappings)
                    m.Create(cfg);
            });

            config.AssertConfigurationIsValid();

            container.Register(() => config.CreateMapper());
        }

        public static void RegisterServices(Container container)
        {
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<ICompanyService, CompanyService>(Lifestyle.Scoped);
        }
    }
}