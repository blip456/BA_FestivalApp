using FestivalLib.model;
using SSA_FestivalApp.Models._DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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

        [AllowAnonymous]
        public ActionResult RSS()
        {
            List<RssItem> lst = RssItem.GetRssItems();
           // RssItem.GetRssItems();
            return View();
        }
    }
}
