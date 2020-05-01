using AutoMapper;
using ClosedXML.Excel;
using Core.Interfaces;
using Data;
using Domain;
using ExcelDataReader;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using ShoppingPortal.Models;
using ShoppingPortal.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoppingPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;
        private ApplicationUserManager _userManager;

        public UserController(IUserRepository repo, ApplicationUserManager userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string userId = "")
        {
            if (userId != "")
            {
               var result = _repo.Delete(userId);
                if (result)
                {
                   var user= _userManager.FindById(userId); 
                    if(user != null)
                    {
                        _userManager.Delete(user);
                    }
                }
            }

            return Json(new { Status = true, Message = "User deleted Successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUser()
        {

            UserDetail users = new UserDetail();
            var data = _repo.GetUsers();
            List<UserModel> user = Mapper.Map<List<UserDetail>, List<UserModel>>(data);
            //Handler the data which is passed from view.
            return Json(new { data = user }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult LoadUserForm(string userId = "")
        {
            var model = new UserModel();

            if (userId != "" && userId != null)
            {
                var user = _repo.GetUser(userId);
                model = Mapper.Map<UserDetail, UserModel>(user);
            }


            return PartialView("UserDetail", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(UserModel userDetail)
        {
            try
            {
                string password = null;
                var model = new UserDetail();

                model = Mapper.Map<UserModel, UserDetail>(userDetail);

                if (model.UserId == null || model.UserId == "")
                {
                    bool includeLowercase = true;
                    bool includeUppercase = true;
                    bool includeNumeric = true;
                    bool includeSpecial = true;
                    bool includeSpaces = false;
                    int lengthOfPassword = 12;

                    password = PasswordGenerator.PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);

                    if (model != null)
                    {
                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                        var result = _userManager.Create(user, password);

                        if (result.Succeeded)
                        {
                            model.UserId = user.Id;

                            _userManager.AddToRole(user.Id, "User");
                            model = _repo.AddOrUpdate(model);

                            var provider = new DpapiDataProtectionProvider("SampleAppName");

                            _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                                provider.Create("SampleTokenName"));

                            string code = _userManager.GeneratePasswordResetToken(user.Id);
                            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                            EmailManager emailManager = new EmailManager();
                            emailManager.SendEmail("Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", user.Email);

                        }
                    }
                }
                else
                {
                    model = _repo.AddOrUpdate(model);
                }

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> ImportFile(HttpPostedFileBase importFile)
        {
            var userList = new List<UserDetail>();
            DataTable dt = new DataTable();
            if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });

            bool includeLowercase = true;
            bool includeUppercase = true;
            bool includeNumeric = true;
            bool includeSpecial = true;
            bool includeSpaces = false;
            int lengthOfPassword = 12;

            try
            {
                //Check file type 
                if (importFile.FileName.EndsWith(".xls") || importFile.FileName.EndsWith(".xlsx"))
                {
                    dt = GetDataFromExcelFile(importFile.InputStream);
                }

                else if (importFile.FileName.EndsWith(".csv"))
                {
                    dt = GetDataFromCSVFile(importFile.InputStream);
                }
                else
                {
                    return Json(new { Status = 1, Message = "Invalid File Type" });
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow objDataRow in dt.Rows)
                    {
                        if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;

                        string Email = objDataRow["Email"].ToString();
                        string password = PasswordGenerator.PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);

                        var user = new ApplicationUser { UserName = Email, Email = Email };
                        var result = _userManager.Create(user, password);

                        if (result.Succeeded)
                        {
                            _userManager.AddToRole(user.Id, "User");

                            userList.Add(new UserDetail()
                            {
                                UserId = user.Id,
                                FirstName = objDataRow["FirstName"].ToString(),
                                LastName = objDataRow["LastName"].ToString(),
                                MiddleName = objDataRow["MiddleName"].ToString(),
                                PhoneNumber = objDataRow["PhoneNumber"].ToString(),
                                Email = Email
                            });
                        }
                    }

                    if (userList.Count > 0)
                    {
                        var result = _repo.AddUsers(userList);
                    }
                }
                
                return Json(new { Status = 1, Message = "File Imported Successfully " });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateStatus(string userId,bool status)
        {
            try
            {
                var result = _repo.UpdateStatus(userId, status);

                return Json(new { Status = 1, Message = "Status Update Successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult LoadUserDetail(string userId)
        {
            UserDetails userDetails = new UserDetails();
            var model = new UserModel();

            if (userId != "" && userId != null)
            {
                var user = _repo.GetUser(userId);
                model = Mapper.Map<UserDetail, UserModel>(user);
                var role = _userManager.GetRoles(userId);
                model.UserRole = role[0];

                userDetails.userDetail = model;
            }

            return PartialView("UserInfo", userDetails);
        }

        private DataTable GetDataFromCSVFile(Stream stream)
        {
            var userList = new List<UserModel>();
            DataTable dt = new DataTable();
            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // To set First Row As Column Names  
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        dt = dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return dt;
        }
        private DataTable GetDataFromExcelFile(Stream stream)
        {
            var userList = new List<UserModel>();

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);

                DataTable dt = new DataTable();

                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }
                return dt;
            }
        }
    }
}