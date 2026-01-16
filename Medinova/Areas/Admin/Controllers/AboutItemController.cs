using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class AboutItemController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values = context.AboutItems.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.AboutItems.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(AboutItem aboutItem)
        {
            var value = context.AboutItems.Find(aboutItem.AboutItemId);
            value.Icon = aboutItem.Icon;
            value.Name = aboutItem.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AboutItem aboutItem)
        {
            context.AboutItems.Add(aboutItem);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var value = context.AboutItems.Find(id);
            context.AboutItems.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}