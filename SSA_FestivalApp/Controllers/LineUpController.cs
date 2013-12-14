using FestivalLib.model;
using SSA_FestivalApp.Models._DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Controllers
{
    public class LineUpController : Controller
    {
        //
        // GET: /LineUp/

        public ActionResult Index()
        {
            ObservableCollection<LineUp> lstLineUps = LineUpRepository.GetLineUps();
            return View(lstLineUps);
        }

    }
}
