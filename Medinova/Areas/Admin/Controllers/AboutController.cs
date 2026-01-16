using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class AboutController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {            
            var about = context.Abouts.FirstOrDefault();

            if (about == null)
            {               
                return View(new About());
            }

            return View(about);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(About model)
        {
            if (ModelState.IsValid)
            {
                if (model.AboutId == 0)
                {                    
                    context.Abouts.Add(model);
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