using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class ServiceController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var services = context.Services.ToList();
            return View(services);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Service service)
        {

            context.Services.Add(service);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var service = context.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
        [HttpPost]
        public ActionResult Update(Service service)
        {

            var value = context.Services.Find(service.ServiceId);
            value.Title = service.Title;
            value.Description = service.Description;
            value.Icon = service.Icon;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var service = context.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            context.Services.Remove(service);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}