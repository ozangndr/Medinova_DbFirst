using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class DoctorController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index()
        {
            var values = context.Doctors.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var departments = context.Departments.ToList();
            ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medinova.Models.Doctor doctor, string Password, string FirstName, string LastName)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Önce Users tablosuna kayıt atıyoruz
                        var newUser = new Medinova.Models.User
                        {
                            UserName = FirstName+LastName,
                            Password = Password,
                            FirtName = FirstName, 
                            LastName = LastName,  
                            UserRole = 2
                        };

                        context.Users.Add(newUser);
                        context.SaveChanges();

                        // 2. Doktorun FullName alanını birleştiriyoruz
                        doctor.FullName = FirstName + " " + LastName;

                        // 3. Oluşan UserId'yi doktora bağlıyoruz
                        doctor.UserId = newUser.UserId;

                        context.Doctors.Add(doctor);
                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        ModelState.AddModelError("", "Hata: " + ex.Message);
                    }
                }
            }
            ViewBag.DepartmentId = new SelectList(context.Departments.ToList(), "DepartmentId", "Name", doctor.DepartmentId);
            return View(doctor);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.Doctors.Find(id);
            ViewBag.DepartmentList = new SelectList(context.Departments.ToList(), "DepartmentId", "Name",value.DepartmentId);            
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(Doctor doctor)
        {
            var value = context.Doctors.Find(doctor.DoctorId);
            value.FullName = doctor.FullName;
            value.ImageUrl = doctor.ImageUrl;
            value.DepartmentId = doctor.DepartmentId;
            value.Description = doctor.Description;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var value = context.Doctors.Find(id);
            context.Doctors.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}