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
<<<<<<< HEAD
            return View(festival);
=======

            return View();
>>>>>>> parent of c38f5ac... SSA: Semi working Line Up view
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
