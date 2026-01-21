using Medinova.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Areas.Admin.Controllers
{
    public class PriceItemController : Controller
    {
        MedinovaContext context = new MedinovaContext();
        public ActionResult Index(int id)
        {

            var values = context.PriceItems.Where(x => x.PriceId == id && x.Price.IsActive==true).ToList();
            ViewBag.pricetitle=values.FirstOrDefault()?.Price.Title;
            ViewBag.ActivePriceId = id;
            return View(values);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.priceId = id;
            return View();
        }
        [HttpPost]
        public ActionResult Create(PriceItem item)
        {
            context.PriceItems.Add(item);
            context.SaveChanges();
            return RedirectToAction("Index", new { id = item.PriceId });
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var value = context.PriceItems.Find(id);
            return View(value);
        }
        [HttpPost]
        public ActionResult Update(PriceItem item)
        {
            var value = context.PriceItems.Find(item.PriceItemId);
            value.PriceId = item.PriceId;
            value.Description = item.Description;
            context.SaveChanges();
            return RedirectToAction("Index", new {id=item.PriceId});
        }
        public ActionResult Delete(int id)
        {
            // 1. Kaydı bul
            var value = context.PriceItems.Find(id);

            // 2. Kayıt gerçekten var mı kontrol et
            if (value != null)
            {
                // Geri döneceğimiz ID'yi güvenli bir şekilde alalım
                // Eğer PriceId nullable ise direkt atama yapabiliriz
                int? priceId = value.PriceId;

                // 3. Silme işlemini yap
                context.PriceItems.Remove(value);
                context.SaveChanges();

                // 4. İlgili paketin index sayfasına ID ile dön
                return RedirectToAction("Index", new { id = priceId });
            }

            // Eğer kayıt bulunamadıysa (örneğin sayfa açıkken başkası sildiyse)
            // Ana teklif listesine yönlendirerek hatayı savuştur
            return RedirectToAction("Index", "Price");
        }
    }
}