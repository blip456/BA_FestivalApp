using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;

namespace Ypsilon2.model
{
    public class LineUp
    {
        #region prop en field

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private DateTime _from;

        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }

        private DateTime _until;

        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }

        private Stage _stage;

        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private Band _band;

        public Band Band
        {
            get { return _band; }
            set { _band = value; }
        }


        private double _timespan;

        public double TimeSpan
        {
            get
            {
                _timespan = calculateTimespan(this.From, this.Until);
                return _timespan;
            }
            set { _timespan = value; }
        }


        #endregion

        #region SQL
        private static LineUp CreateLineUp(DbDataReader reader)
        {
            LineUp lineup = new LineUp();
            lineup.ID = Convert.ToString(reader["lineup_id"]);
            lineup.Date = Convert.ToDateTime(reader["lineup_date"]);
            lineup.From = Convert.ToDateTime(reader["lineup_from"]);
            lineup.Until = Convert.ToDateTime(reader["lineup_until"]);
            //lineup.Stage = //een methode die de stage name ophaalt -> enkel name (string) is genoeg
            //lineup.Band = //een methode die get bands by id oproept
            lineup.Band = Band.GetBandByID(lstBands, Convert.ToInt32(reader["lineup_band"]));
            return lineup;
        }

        public static ObservableCollection<LineUp> GetBandsByLineUpIDAndDate(int id, DateTime date)
        {
            string dag = Convert.ToString(date);
            string[] arrDate = dag.Split(' ');

            ObservableCollection<LineUp> lstGevondenLineUps = new ObservableCollection<LineUp>();
            DbDataReader reader = Database.GetData("SELECT * FROM lineup WHERE lineup_stage = " + id + " AND  lineup_date='" + date.ToString("yyyy-MM-dd HH:mm:ss") + "';");
            //DbDataReader reader = Database.GetData("SELECT * FROM lineup WHERE lineup_stage = " + id + " AND  lineup_date='2013-12-27';");

            while (reader.Read())
            {
                lstGevondenLineUps.Add(CreateLineUp(reader));
            }
            //lstGevondenLineUps.OrderBy(i => i.From);

            lstGevondenLineUps.OrderByDescending(i => i.From);
            return lstGevondenLineUps;
        }

        public static ObservableCollection<LineUp> GetBandsByLineUpID(int id)
        {
            ObservableCollection<LineUp> lstGevondenLineUps = new ObservableCollection<LineUp>();
            DbDataReader reader = Database.GetData("SELECT * FROM lineup WHERE lineup_stage = " + id + ";");
            while (reader.Read())
            {
                lstGevondenLineUps.Add(CreateLineUp(reader));
            }

            return lstGevondenLineUps;
        }

        public static ObservableCollection<LineUp> GetLineUps()
        {
            ObservableCollection<LineUp> lstLineUps = new ObservableCollection<LineUp>();
            string sql = "SELECT * FROM lineup";
            DbDataReader reader = Database.GetData(sql);
            while (reader.Read())
            {
                lstLineUps.Add(CreateLineUp(reader));
            }
            return lstLineUps;
        }

        public static LineUp GetLineUpByID(int id)
        {
            LineUp gevondenLineUp = new LineUp();
            string sql = "SELECT * FROM lineup WHERE lineup_id = " + id + "";
            DbDataReader reader = Database.GetData(sql);
            while (reader.Read())
            {
                gevondenLineUp = CreateLineUp(reader);
            }

            return gevondenLineUp;
        }

        public static void AddLineUp(LineUp lineup)
        {
            string sql = "INSERT INTO lineup(lineup_date, lineup_from, lineup_until, lineup_stage, lineup_band) VALUES (@date, @from, @until, @stageid, @bandid);";

            DbParameter par1 = Database.AddParameter("@date", lineup.Date);
            DbParameter par2 = Database.AddParameter("@from", lineup.From);
            DbParameter par3 = Database.AddParameter("@until", lineup.Until);
            DbParameter par4 = Database.AddParameter("@stageid", lineup.Stage.ID);
            DbParameter par5 = Database.AddParameter("@bandid", lineup.Band.ID);

            int i = Database.ModifyData(sql, par1, par2, par3, par4, par5);
            Console.WriteLine(i + " row(s) are affected");
        }
        #endregion

        #region Methods

        private static ObservableCollection<Band> lstBands = Band.GetBands();

        private static ObservableCollection<Ypsilon2.model.LineUp> lstAlleLineUps = GetLineUps();

        //berekend hoeveel tijd er tussen het begin en einde van een optreden zit, dit wordt oa gebruikt voor de breedte van de stackpanel te bepalen
        public static double calculateTimespan(DateTime dtFrom, DateTime dtUntil)
        {
            string sFrom = dtFrom.ToString("HH:mm");
            string sUntil = dtUntil.ToString("HH:mm");

            string[] arrsSplitFrom = sFrom.Split(':');
            double dFromHour = Convert.ToDouble(arrsSplitFrom[0]);
            double dFromMinute = Convert.ToDouble(arrsSplitFrom[1]);
            double dFrom = dFromHour + (dFromMinute / 60);

            string[] arrsSplitUntil = sUntil.Split(':');
            double dUntilHour = Convert.ToDouble(arrsSplitUntil[0]);
            double dUntilMinute = Convert.ToDouble(arrsSplitUntil[1]);
            double dUntil = dUntilHour + (dUntilMinute / 60);

            double dTimespan = (dUntil - dFrom) * 150;

            return dTimespan;
        }

        #endregion
    }
}
