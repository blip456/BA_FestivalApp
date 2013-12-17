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

        public static ObservableCollection<LineUp> GetLineUpsByDay(int iPod, DateTime datum)
        {            
            ObservableCollection < LineUp > lsLineUps = LineUp.GetBandsByLineUpIDAndDate(iPod, datum);
            return lsLineUps;
        }

        public static ObservableCollection<DateTime> GetDagen()
        {
            ObservableCollection<DateTime> lst = Festival.aantalDagen();
            return lst;
        }

        public static ObservableCollection<Stage> GetStages()
        {
            ObservableCollection<Stage> lst = Stage.GetAlleStages();
            return lst;
        }
    }
}