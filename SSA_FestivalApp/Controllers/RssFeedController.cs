using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Controllers
{
    public class RssFeedController : Controller
    {
        //
        // GET: /RssFeed/

        [HttpPost]
        public ActionResult InsertRSS(RssItem item)
        {            
            RssItem.AddRssItem(item);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
