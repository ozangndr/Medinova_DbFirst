using Medinova.DTOs;
using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDto loginDto)
        {
            var user = context.Users
                .FirstOrDefault(u => u.UserName == loginDto.Username && u.Password == loginDto.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Adı Veya Şifre Hatalı");
                return View(loginDto);
            }
            FormsAuthentication.SetAuthCookie(user.UserName, false);
            Session["UserName"] = user.FirtName + " " + user.LastName;
            Session["UserId"] = user.UserId;
            var userRole = context.UserRoles.Find(user.UserRole);
            string roleName = userRole.RoleName;
            Session["UserRole"] = roleName;

            if (roleName == "Admin")
            {
                // Admin Areasındaki About Controller -> Index Action
                return RedirectToAction("Index", "About", new { area = "Admin" });
            }
            else if (roleName == "Doctor")
            {
                // DoctorArea Areasındaki Appointment Controller -> Index Action
                return RedirectToAction("Index", "Appointment", new { area = "DoctorArea" });
            }

            // Eğer rol eşleşmezse varsayılan bir yere yönlendir
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

    }
}