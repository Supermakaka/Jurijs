using BusinessLogic;
using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.BusinessLogic.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        public IUserService userService;

        [TestInitialize]
        public void TestInitialize()
        {
            this.userService = new UserService(new InMemoryDataContext());
           
            CreateUserWithCompany();
        }

        public void CreateUserWithCompany()
        {
            var company = new Company()
            {
                Name = "testCompany"
            };

            var user = new User()
            {
                CreateDate = DateTime.Now,
                Disabled = false,
                Email = "testUser@example.com",
                FullName = "Test User",
                Company = company
            };

            userService.Add(user);
        }

        [TestMethod]
        public void Password_Is_Incorrect_User_Is_Not_Valid()
        {
            //Assert.IsFalse(userService.IsValid("testUser@example.com", string.Empty));
        }

        [TestMethod]
        public void Password_Is_Correct_User_Is_Valid()
        {
            //Assert.IsTrue(userService.IsValid("testUser@example.com", "12345"));
        }

        [TestMethod]
        public void User_Was_Enabled_State_Changed_To_Disabled()
        {
            var user = userService.Get(u => !u.Disabled);
            userService.DisableOrEnable(user);
            Assert.IsTrue(user.Disabled);
        }

        [TestMethod]
        public void Users_With_Companies_Count_More_Than_0()
        {
            var userList = userService.GetAllWithCompanies().ToList();
            Assert.IsTrue(userList.Count > 0);
        }
    }
}
