using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
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

        private string _omschrijving;

        public string Omschrijving
        {
            get { return _omschrijving; }
            set { _omschrijving = value; }
        }
        

        #endregion 


        #region SQL

        public static Festival GetFestivals()
        {
            Festival festival = new Festival();
            DbDataReader reader = Database.GetData("SELECT * FROM festival");
            while (reader.Read())
            {
                festival.StartDate = Convert.ToDateTime(reader["festival_start"]);
                festival.EndDate = Convert.ToDateTime(reader["festival_end"]);
                festival.Name = Convert.ToString(reader["festival_name"]);
                festival.ID = Convert.ToInt32(reader["festival_id"]);
                festival.Omschrijving = (string)reader["festival_omschrijving"];
            }
            return festival;
        }       

        public static void EditFestival(Festival festival)
        {
            string sql = "UPDATE festival SET festival_start=@begin, festival_end=@end, festival_name=@name, festival_omschrijving=@omschrijving WHERE festival_id=@ID;";

            DbParameter par1 = Database.AddParameter("@begin", festival.StartDate);
            DbParameter par2 = Database.AddParameter("@end", festival.EndDate);
            DbParameter par3 = Database.AddParameter("@name", festival.Name);
            DbParameter par4 = Database.AddParameter("@omschrijving", festival.Omschrijving);

            DbParameter parID = Database.AddParameter("@ID", festival.ID);

            int i = Database.ModifyData(sql, par1, par2, par3, par4, parID);
            if (i == 0)
            {
                MessageBox.Show("Opslaan mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
            }
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
