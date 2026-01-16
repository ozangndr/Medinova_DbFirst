using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Medinova.Controllers
{
    [Authorize]
    public class AdminAboutController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}