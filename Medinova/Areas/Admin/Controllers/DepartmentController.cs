using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Admin/Department

        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values= context.Departments.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Department department)
        {
            context.Departments.Add(department);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.Departments.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(Department department)
        {
            var value = context.Departments.Find(department.DepartmentId);
            value.Name = department.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var value = context.Departments.Find(id);
            context.Departments.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}