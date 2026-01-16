using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult AssignRole(int UserId, int UserRole)
        {
            var user = context.Users.Find(UserId);
            if (user != null)
            {
                user.UserRole = UserRole;
                context.SaveChanges();
                TempData["Success"] = "Rol başarıyla güncellendi.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Profile()
        {
            int id = Convert.ToInt32(Session["UserId"]);
            var user = context.Users.Where(x => x.UserId == id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(Medinova.Models.User updatedUser, string NewPassword)
        {
            int sessionUserId = Convert.ToInt32(Session["UserId"]);

            // Güvenlik: Sadece kendi profilini güncelleyebilir
            var user = context.Users.FirstOrDefault(x => x.UserId == sessionUserId);

            if (user != null)
            {
                user.FirtName = updatedUser.FirtName;
                user.LastName = updatedUser.LastName;
                user.UserName = updatedUser.UserName;

                // Eğer yeni şifre alanı doluysa şifreyi güncelle
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    user.Password = NewPassword;
                }

                context.SaveChanges();
                TempData["Success"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("Profile");
            }

            return View(updatedUser);
        }
    }
}