using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        // GET: Admin/Banner
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var banner = context.Banners.FirstOrDefault();

            if (banner == null)
            {
                return View(new Banner());
            }

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Banner model)
        {
            if (ModelState.IsValid)
            {
                if (model.BannerId == 0)
                {
                    context.Banners.Add(model);
                }
                else
                {
                    context.Entry(model).State = EntityState.Modified;
                }

                context.SaveChanges();
                TempData["Success"] = "Bilgiler başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}