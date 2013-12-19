using FestivalLib.model;
using SSA_FestivalApp.Models._DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            Festival festival = HomeRepository.GetFestival();
            
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View(festival);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
