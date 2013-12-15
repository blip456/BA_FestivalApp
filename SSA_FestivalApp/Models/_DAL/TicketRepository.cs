using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSA_FestivalApp.Models._DAL
{
    public class TicketRepository
    {
        public static void ReserveerTicket(Ticket ticket)
        {
            Ticket.AddTicket(ticket);
        }
    }
}