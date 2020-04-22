using AutoMapper;
using Core.Interfaces;
using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }


        // GET: User
        public ActionResult Index()
        {
            var data = _repo.GetUsers();

            List<UserModel> users = Mapper.Map<List<UserDetail>, List<UserModel>>(data);
            return View(users);
        }

        [HttpPost]
        public ActionResult Delete(string userId = "")
        {
            bool status = false;
            if (userId != "")
            {
                status = _repo.Delete(userId);
            }

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RefreshDiv2()
        {

            UserDetail users = new UserDetail();
            var data = _repo.GetUsers();
            List<UserModel> user = Mapper.Map<List<UserDetail>, List<UserModel>>(data);
            //Handler the data which is passed from view.
            return Json(user, JsonRequestBehavior.AllowGet); //Send the data back to view.
        }
    }
}