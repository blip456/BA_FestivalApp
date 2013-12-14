using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace SSA_FestivalApp.Models._DAL
{
    public class LineUpRepository
    {
        public static ObservableCollection<LineUp> GetLineUps()
        {
            ObservableCollection<LineUp> lstLineUps = LineUp.GetLineUps();
            return lstLineUps;
        }
    }
}