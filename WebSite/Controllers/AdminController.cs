using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;

using AutoMapper;

using DataTables.AspNet.AspNet5.Extensions.Linq;

using BusinessLogic.Services;
using BusinessLogic.Models;

namespace WebSite.Controllers
{
    using Core.Identity;
    using ViewModels.Admin;
    using ViewModels.Mapping;

    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private const int PageSize = 10;

        private IdentityUserManager userManager;
        private IdentityRoleManager roleManager;
        private IdentitySignInManager signInManager;

        private IUserService userService;
        private ICompanyService companyService;

        IMapper mapper;

        public AdminController(IdentityUserManager userManager, IdentityRoleManager roleManager, IdentitySignInManager signInManager, IUserService userService, ICompanyService companyService, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

            this.userService = userService;
            this.companyService = companyService;

            this.mapper = mapper;
        }

        public ActionResult UserList()
        {
            var model = new UserListViewModel();
            model.Roles = roleManager.Roles.OrderBy(r => r.Name).ToList().ToSelectListItems(r => r.Name, r => r.Id, true);

            return View(model);
        }

        public ActionResult UserListAjax(IDataTablesRequest request)
        {
            var users = userService.GetAllWithCompanies();

            var res = request.ApplyToQuery(users, opt => opt
                .ForColumn("Email")
                    .EnableGlobalSearch()
                .ForColumn("FullName")
                    .EnableGlobalSearch()
                .ForColumn("CompanyName")
                    .MapToProperty(u => u.Company.Name)
                    .EnableGlobalSearch()
                .ForColumn("Role")
                    .SearchUsing((u, val) => u.Roles.Any(r => r.RoleId.ToString() == val))
                    .IgnoreWhenSorting()
            );

            var model = mapper.Map<IEnumerable<UserListDatatableViewModel>>(res.QueryFiltered);

            var response = DataTablesResponse.Create(request, res.TotalRecords, res.TotalRecordsFiltered, model);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyList()
        {
            return View();
        }

        public ActionResult CompanyListAjax(IDataTablesRequest request)
        {
            var companies = companyService.GetAll();

            var res = request.ApplyToQuery(companies);

            var model = mapper.Map<IEnumerable<CompanyListDatatableViewModel>>(res.QueryFiltered);

            var response = DataTablesResponse.Create(request, res.TotalRecords, res.TotalRecordsFiltered, model);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisableOrEnableUser(int id)
        {
            var user = userService.GetById(id);

            userService.DisableOrEnable(user);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmDeleteUser(int id)
        {
            var user = userService.GetById(id);

            var model = mapper.Map<UserFormViewModel>(user);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            userService.Delete(userService.GetById(id));

            return Json(new { success = true });
        }

        public ActionResult CreateUser()
        {
            var companies = companyService.GetAll().ToList();

            var model = new UserFormViewModel();
            model.Companies = companies.ToSelectListItems(r => r.Name, r => r.Id, true);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateUser(UserFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User();

                newUser.UserName = model.Email;
                newUser.CompanyId = model.CompanyId;
                newUser.FullName = model.FullName;
                newUser.Email = model.Email;
                newUser.Disabled = model.Disabled;
                newUser.CreateDate = DateTime.Now;

                var result = userManager.CreateAsync(newUser, model.Password).Result;

                if (result.Succeeded)
                {
                    result = userManager.AddToRoleAsync(newUser.Id, RoleNames.User).Result;

                    if (result.Succeeded)
                        return Json(new { success = true });
                }

                CollectIdentityErrors(result);
            }

            var m = new UserFormViewModel();
            m.Companies = companyService.GetAll().ToList().ToSelectListItems(r => r.Name, r => r.Id, true);

            return PartialView(m);
        }

        public ActionResult EditUser(int id)
        {
            var user = userService.GetById(id);

            var model = mapper.Map<UserFormViewModel>(user);
            model.Companies = companyService.GetAll().ToList().ToSelectListItems(r => r.Name, r => r.Id, true);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditUser(UserFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userToEdit = userManager.FindByIdAsync(model.Id.Value).Result;

                userToEdit.UserName = model.Email;
                userToEdit.CompanyId = model.CompanyId;
                userToEdit.FullName = model.FullName;
                userToEdit.Email = model.Email;
                userToEdit.Disabled = model.Disabled;

                var result = userManager.UpdateAsync(userToEdit).Result;

                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(model.ConfirmPassword))
                    {
                        string resetToken = userManager.GeneratePasswordResetTokenAsync(userToEdit.Id).Result;
                        result = userManager.ResetPasswordAsync(userToEdit.Id, resetToken, model.ConfirmPassword).Result;

                        if (result.Succeeded)
                            return Json(new { success = true });
                    }
                    else
                        return Json(new { success = true });
                }

                CollectIdentityErrors(result);
            }

            var m = new UserFormViewModel();
            m.Companies = companyService.GetAll().ToList().ToSelectListItems(r => r.Name, r => r.Id, true);

            return PartialView(m);
        }

        public ActionResult EditCompany(int? id)
        {
            var company = companyService.GetById(id.Value);

            var model = mapper.Map<CompanyFormViewModel>(company);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyFormViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);
            
            var companyToEdit = companyService.GetById(model.Id.Value);

            companyToEdit.Name = model.Name;

            companyService.Update(companyToEdit);

            return Json(new { success = true });
        }

        public ActionResult CreateCompany()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateCompany(CompanyFormViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView();

            var newCompany = new Company();

            newCompany.Name = model.Name;

            companyService.Add(newCompany);

            return Json(new { success = true });
        }

        [HttpGet]
        public ActionResult ConfirmDeleteCompany(int id)
        {
            var company = companyService.GetById(id);

            var model = mapper.Map<CompanyFormViewModel>(company);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DeleteCompany(int id)
        {
            var company = companyService.GetById(id);

            if (!company.Users.Any(u => u.Company == company))
            {
                companyService.Delete(company);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Cannot delete company because company has users." });
            }
        }

        private void CollectIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}