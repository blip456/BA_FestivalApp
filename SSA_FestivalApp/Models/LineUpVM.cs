using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSA_FestivalApp.Models
{
    public class LineUpVM
    {
        public LineUp LineUp { get; set; }
        public int SelectedPod { get; set; }        
        public DateTime SelectedDag { get; set; }
        public SelectList lstDagen { get; set; }
        public SelectList lstPodia { get; set; }
        public ObservableCollection<LineUp> LineUps { get; set; }
    }
}