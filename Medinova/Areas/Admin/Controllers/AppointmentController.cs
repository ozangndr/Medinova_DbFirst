using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Admin/Appointment
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index(int? doctorId, DateTime? date)
        {
            // Doktorları filtre dropdown'ı için gönderiyoruz
            ViewBag.DoctorList = new SelectList(context.Doctors.ToList(), "DoctorId", "FullName");

            // Temel sorgu (İlişkili tabloları dahil ederek)
            var appointments = context.Appointments
                                      .Include(a => a.Doctor)
                                      .Include(a => a.Doctor.Department)
                                      .AsQueryable();

            // Filtreleme mantığı
            if (doctorId.HasValue)
            {
                appointments = appointments.Where(a => a.DoctorId == doctorId);
            }

            if (date.HasValue)
            {
                DateTime filterDate = date.Value.Date;

                appointments = appointments.Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == filterDate);
            }

            return View(appointments.OrderByDescending(a => a.AppointmentDate).ToList());
        }
    }
}