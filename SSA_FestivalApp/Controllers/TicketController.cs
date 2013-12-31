using FestivalLib.model;
using SSA_FestivalApp.Models;
using SSA_FestivalApp.Models._DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/       
        
        public ActionResult Reserveer()
        {
            TicketTypeVM vm = new TicketTypeVM();            
            vm.lstTypes = new SelectList(TicketRepository.GetTicketTypes().ToList(), "ID", "NameCat");
            return View("Reserveer", vm);
        }

        [HttpPost]
        public ActionResult Insert(TicketTypeVM vm)
        {
            Ticket ticket = vm.Ticket;
            ticket.TicketType = TicketType.GetTicketTypeByID(vm.SelectedType);
            TicketRepository.ReserveerTicket(vm.Ticket); 
            //Ticket.SendMail(vm.Ticket);
            //waarom gebruik ik een tempdata? => tempdata kan itt viebag  of viewdata een redirect overleven 
            //-> viebag/viewdata is voor in de view zelf (tempdata is voor redirect-
            TempData["order"] = vm.Ticket;
            return RedirectToAction("Overview", "Ticket");
        }

        public ActionResult Overview()
        {
            Ticket ticket = (Ticket)TempData["order"];            
            return View(ticket);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Summary()
        {
            ObservableCollection<Ticket> lstAlleTickets = TicketRepository.GetAllTickets();
            return View(lstAlleTickets);
        }

    }
}
