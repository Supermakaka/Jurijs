using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector;
using System.Web.Mvc;
using WebSite;

namespace Tests.WebSite.Tests
{
    [TestClass]
    public class BaseWebSiteTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            var container = new Container();

            SimpleInjectorConfig.RegisterAutomapper(container);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
