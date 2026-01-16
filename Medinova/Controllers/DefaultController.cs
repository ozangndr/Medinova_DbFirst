using Medinova.DTOs;
using Medinova.Enums;
using Medinova.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        MedinovaContext context = new MedinovaContext();


        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult DefaultAppointment()
        {
            var departments = context.Departments.ToList();
            ViewBag.departments = (from department in departments
                                   select new SelectListItem
                                   {
                                       Text = department.Name,
                                       Value = department.DepartmentId.ToString()
                                   }).ToList();

            var dateList = new List<SelectListItem>();
            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(i);
                dateList.Add(new SelectListItem
                {
                    Text = date.ToString("dd.MMMM.dddd"),
                    Value = date.ToString("yyyy-MM-dd")
                });
            }
            ViewBag.dateList = dateList;
            return PartialView();
        }

        [HttpPost]
        public ActionResult MakeAppointment(Appointment appointment)
        {
            appointment.IsActive = true;
            context.Appointments.Add(appointment);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetDoctorsByDepartmentId(int departmentId)
        {
            var doctors = context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new SelectListItem
                {
                    Text = d.FullName,
                    Value = d.DoctorId.ToString()
                }).ToList();
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAvailableHours(DateTime selectedDate, int doctorId)
        {
            var bookedTimes = context.Appointments
                .Where(a => a.AppointmentDate == selectedDate && a.DoctorId == doctorId)
                .Select(a => a.AppointmentTime)
                .ToList();

            var dtolist = new List<AppointmentAvailablitiyDto>();
            foreach (var hour in Times.AppointmentHour)
            {
                var dto = new AppointmentAvailablitiyDto();
                dto.Time = hour;

                if (bookedTimes.Contains(hour))
                {
                    dto.IsBooked = true;
                }
                else
                {
                    dto.IsBooked = false;
                }
                dtolist.Add(dto);
            }
            return Json(dtolist, JsonRequestBehavior.AllowGet);
        }


        //---- PAGES ----//

        [HttpGet]
        public PartialViewResult DefaultBanner()
        {
            var banner = context.Banners.FirstOrDefault();
            return PartialView(banner);
        }
        [HttpGet]
        public PartialViewResult DefaultAbout()
        {
            var items = context.AboutItems.ToList();
            ViewBag.items = items;
            var about = context.Abouts.FirstOrDefault();
            return PartialView(about);
        }

        [HttpGet]
        public PartialViewResult DefaultPricing()
        {
            var value = context.Prices.Include("PriceItems").ToList();
            return PartialView(value);
        }
        [HttpGet]
        public PartialViewResult DefaultService()
        {
            var value = context.Services.ToList();
            return PartialView(value);
        }

        [HttpGet]
        public PartialViewResult DefaultDoctor()
        {
            var value = context.Doctors.ToList();
            return PartialView(value);
        }
        [HttpGet]
        public PartialViewResult DefaultTestimonial()
        {
            var value = context.Testimonials.ToList();
            return PartialView(value);
        }
        [HttpGet]
        public PartialViewResult DefaultBlog()
        {
            var value = context.Blogs.ToList();
            return PartialView(value);
        }
        [HttpGet]
        public PartialViewResult AIRehberi()
        {
            // Kullanıcının en azından hangi bölüme yazacağını seçmesi AI'nın daha doğru cevap vermesini sağlar
            var departments = context.Departments.ToList();
            return PartialView(departments);
        }

        [HttpPost]
        public async Task<JsonResult> GetAIResponse(string message)
        {
            // 1. JSON dosyasının yolunu bul ve oku
            string jsonPath = HttpContext.Server.MapPath("~/secrets.json");
            string jsonContent = System.IO.File.ReadAllText(jsonPath);

            // 2. JSON içinden ApiKey'i ayıkla
            var secrets = JObject.Parse(jsonContent);
            string apiKey = secrets["OpenAI"]["ApiKey"].ToString();

            // 3. OpenAI API Bilgileri
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "system", content = "Sen bir hastane rehberisin. Tıbbi tavsiye vermeden uygun polikliniği öner." },
                new { role = "user", content = message }
            }
                };

                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    string aiText = result.choices[0].message.content;
                    return Json(aiText);
                }

                return Json("Şu an yanıt veremiyorum, lütfen daha sonra tekrar deneyiniz.");
            }
        }
    }
}