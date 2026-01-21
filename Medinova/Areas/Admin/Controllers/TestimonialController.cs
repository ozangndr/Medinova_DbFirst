using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class TestimonialController : Controller
    {
        

        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values = context.Testimonials.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Testimonial testimonial)
        {
            context.Testimonials.Add(testimonial);
            testimonial.UserId = Convert.ToInt32(Session["UserId"].ToString());
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.Testimonials.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(Testimonial testimonial)
        {
            var value = context.Testimonials.Find(testimonial.TestimonialId);
            testimonial.UserId = Convert.ToInt32(Session["UserId"].ToString());
            value.Title = testimonial.Title;
            value.Description = testimonial.Description;
            value.Icon = testimonial.Icon;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var value = context.Testimonials.Find(id);
            context.Testimonials.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}