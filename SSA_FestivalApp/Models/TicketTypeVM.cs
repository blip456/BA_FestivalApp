using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Models
{
    public class TicketTypeVM
    {
        public Ticket Ticket { get; set; }      
        public int SelectedType { get; set; }
        public SelectList lstTypes { get; set; }              
    }
}