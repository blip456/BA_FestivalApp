using Ypsilon2.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;

namespace Ypsilon2.model
{
    public class Festival
    {
        #region Field en properties

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }        

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }


        private DateTime _endDatde;

        public DateTime EndDate
        {
            get { return _endDatde; }
            set { _endDatde = value; }
        }

        #endregion 


        #region SQL
        private static Festival CreateFestival(DbDataReader reader)
        {
            Festival festival = new Festival();
            
            festival.StartDate = Convert.ToDateTime(reader["start_date"]);
            festival.EndDate = Convert.ToDateTime(reader["enddate"]);
           
            return festival;
        }

        public static Festival GetFestivals()
        {
            Festival festival = new Festival();
            DbDataReader reader = Database.GetData("SELECT * FROM festival");
            while (reader.Read())
            {
                festival.StartDate = Convert.ToDateTime(reader["start_date"]);
                festival.EndDate = Convert.ToDateTime(reader["enddate"]);
                festival.Name = Convert.ToString(reader["festivalname"]);
                festival.ID = Convert.ToInt32(reader["festival_id"]);
            }
            return festival;
        }       

        public static void EditFestival(Festival festival)
        {
            string sql = "UPDATE festival SET  start_date=@begin, enddate=@end, festivalname=@name WHERE festival_id=@ID;";

            DbParameter par1 = Database.AddParameter("@begin", festival.StartDate);
            DbParameter par2 = Database.AddParameter("@end", festival.EndDate);
            DbParameter par3 = Database.AddParameter("@name", festival.Name);

            DbParameter parID = Database.AddParameter("@ID", festival.ID);

            int i = Database.ModifyData(sql, par1, par2, par3, parID);
            Console.WriteLine(i + " row(s) are affected");
        }
        #endregion

        public static ObservableCollection<DateTime> aantalDagen()
        {
            ObservableCollection<DateTime> lstDagen = new ObservableCollection<DateTime>();
            Festival festival = GetFestivals();
            TimeSpan timespan = festival.EndDate - festival.StartDate;
            DateTime volgendeDag = festival.StartDate;
            for (int i = 0; i <= timespan.Days+1; i++)
            {
                TimeSpan ts = TimeSpan.FromDays(i);
                volgendeDag = festival.StartDate.Add(ts);
               
                lstDagen.Add(volgendeDag.Date);                
            }

            return lstDagen;
        }

    }
}
