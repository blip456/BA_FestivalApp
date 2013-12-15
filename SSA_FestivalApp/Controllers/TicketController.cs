using FestivalLib.model;
using SSA_FestivalApp.Models._DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reserveer(Ticket ticket)
        {
            TicketRepository.ReserveerTicket(ticket);
            return View();
        }

    }
}
