using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.DoctorArea.Controllers
{
    public class BlogController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values = context.Blogs.Where(x => x.UserId == Convert.ToInt32(Session["UserId"].ToString())).ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Blog blog)
        {
            context.Blogs.Add(blog);
            blog.UserId = Convert.ToInt32(Session["UserId"].ToString());
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.Blogs.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(Blog blog)
        {
            var value = context.Blogs.Find(blog.BlogId);
            blog.UserId = Convert.ToInt32(Session["UserId"].ToString());
            value.Title = blog.Title;
            value.Description = blog.Description;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var value = context.Blogs.Find(id);
            context.Blogs.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}