using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Models
{
    public class LineUpVM
    {
        public LineUp LineUp { get; set; }
        public int SelectedDag { get; set; }
        public SelectList lstDagen { get; set; }  
    }
}