using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class PriceController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values = context.Prices.Where(x=>x.IsActive==true).ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Create()
        {           
            return View();
        }
        [HttpPost]
        public ActionResult Create(Price price)
        {
            context.Prices.Add(price);
            price.IsActive = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.Prices.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(Price price)
        {
            var value = context.Prices.Find(price.PriceId);
            value.Price1 = price.Price1;
            value.Title = price.Title;
            value.IsActive = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var value = context.Prices.Find(id);
            value.IsActive = false;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}