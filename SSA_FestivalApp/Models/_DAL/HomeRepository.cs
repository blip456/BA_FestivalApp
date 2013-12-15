using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSA_FestivalApp.Models._DAL
{
    public class HomeRepository
    {
        public static Festival GetFestival()
        {
            Festival festival = Festival.GetFestivals();
            return festival;
        }
    }
}