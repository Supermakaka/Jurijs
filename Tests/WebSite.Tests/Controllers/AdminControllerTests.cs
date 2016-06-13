using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using AutoMapper;

using BusinessLogic.Services;
using BusinessLogic.Models;

using WebSite.Core.Identity;
using WebSite.Controllers;
using WebSite.ViewModels.Admin;

namespace Tests.WebSite.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests : BaseWebSiteTests
    {
        public IdentityUserManager userManager;
        public IdentityRoleManager roleManager;
        public IdentitySignInManager signInManager;
        public IUserService userService;
        public ICompanyService companyService;
        public IMapper mapper;

        public User CreateUserWithCompany()
        {
            var company = new Company()
            {
                Id = 1,
                Name = "testCompany1"
            };

            var user = new User()
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Disabled = false,
                Email = "testUser@example.com",
                FullName = "TestUser",
                Company = company
            };

            return user;
        }

        public List<Company> CreateCompanies()
        {
            return new List<Company>
            {
                new Company()
                {
                    Id = 1,
                    Name = "TestCompany1"
                },
                new Company()
                {
                    Id = 2,
                    Name = "TestCompant2"
                }
            };
        }

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();

            userManager = MockRepository.GenerateStub<IdentityUserManager>();
            roleManager = MockRepository.GenerateStub<IdentityRoleManager>();
            signInManager = MockRepository.GenerateStub<IdentitySignInManager>();
            userService = MockRepository.GenerateStub<IUserService>();
            companyService = MockRepository.GenerateStub<ICompanyService>();
            mapper = MockRepository.GenerateStub<IMapper>();
        }

        [TestMethod]
        public void Test_UserList_View()
        {
            var user = CreateUserWithCompany();
            userService.Stub(x => x.GetAllWithCompanies()).Return(new List<User> { user }.AsQueryable());
  
            var adminController = new AdminController(userManager, roleManager, signInManager, userService, companyService, mapper);
            
            var viewResult = adminController.UserList() as ViewResult;
            var dataModel = ((IEnumerable<UserListViewModel>)viewResult.ViewData.Model).ToList();
           
            Assert.AreEqual(dataModel.Count, 1);
        }

        [TestMethod]
        public void Test_DisableOrEnableUser_View()
        {
            var user = CreateUserWithCompany();
            
            userService.Stub(x => x.GetById(1)).Return(user);
            userService.Expect(x => x.DisableOrEnable(user)).WhenCalled(c=> user.Disabled = true);

            var adminController = new AdminController(userManager, roleManager, signInManager, userService, companyService, mapper);
            var viewResult = adminController.DisableOrEnableUser(user.Id) as PartialViewResult;
            var dataModel = (User)viewResult.ViewData.Model;

            Assert.AreEqual("UserListRow", viewResult.ViewName);
            Assert.IsTrue(dataModel.Disabled);
        }

        [TestMethod]
        public void Test_EditUser_View()
        {
            var user = CreateUserWithCompany();
            var companies = CreateCompanies();

            userService.Stub(x => x.GetById(1)).Return(user);
            companyService.Stub(x => x.GetAll().ToList()).Return(companies);

            var adminController = new AdminController(userManager, roleManager, signInManager, userService, companyService, mapper);
            var viewResult = adminController.EditUser(user.Id) as ViewResult;
            var dataModel = (UserFormViewModel)viewResult.ViewData.Model;

            Assert.AreEqual("testUser@example.com", dataModel.Email);
            Assert.AreEqual(2, dataModel.Companies.Count());
        }

        [TestMethod]
        public void Test_EditUser_View_Post()
        {
            var updatedUser = new User();
            var user = CreateUserWithCompany();

            userService.Stub(x => x.GetById(1)).Return(user);
            userService.Expect(x => x.Update(user)).WhenCalled(c => updatedUser = user);

            companyService.Stub(x => x.GetById(2)).Return(new Company() { Id = 2, Name = "TestCompany2" });

            var editUserViewModel = new UserFormViewModel()
            {
                Id = 1,
                CompanyId = 2,
                Disabled = true,
                Email = "testUser2@example.com",
                FullName = "TestUser2"
            };


            var adminController = new AdminController(userManager, roleManager, signInManager, userService, companyService, mapper);


            var result = (RedirectToRouteResult)adminController.EditUser(editUserViewModel);

            Assert.AreEqual(updatedUser.Email, "testUser2@example.com");
            Assert.AreEqual(updatedUser.FullName, "TestUser2");
            Assert.AreEqual(updatedUser.Company.Name, "TestCompany2");
            Assert.IsTrue(updatedUser.Disabled);
            Assert.AreEqual("UserList", result.RouteValues["Action"]);
            
        }
    }
}
