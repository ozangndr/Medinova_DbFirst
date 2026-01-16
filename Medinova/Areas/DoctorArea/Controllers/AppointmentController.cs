using Medinova.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.DoctorArea.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Admin/Appointment
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index(DateTime? date)
        {
            // 1. Session kontrolü: Kullanıcı login değilse yönlendir
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            // 2. Doktoru bul
            var doctor = context.Doctors.FirstOrDefault(d => d.UserId == userId);

            // HATA ÖNLEME: Eğer giriş yapan kullanıcı bir doktor değilse (doctor null ise)
            if (doctor == null)
            {
                // İstersen hata mesajı bas, istersen boş liste döndür
                TempData["Error"] = "Doktor profiliniz bulunamadı.";
                return View(new List<Appointment>());
            }

            // 3. Sorguyu başlat
            var appointments = context.Appointments
                                      .Where(a => a.DoctorId == doctor.DoctorId)
                                      .Include(a => a.Doctor)
                                      .Include(a => a.Doctor.Department)
                                      .AsQueryable();

            // 4. Tarih filtresi
            if (date.HasValue)
            {
                DateTime filterDate = date.Value.Date;
                appointments = appointments.Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == filterDate);
            }

            // 5. Veriyi gönder (ToList öncesi OrderBy ekledik)
            var result = appointments.OrderByDescending(a => a.AppointmentDate).ToList();

            return View(result);
        }
    }
}