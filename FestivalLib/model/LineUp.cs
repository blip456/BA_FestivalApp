using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class LineUp : IDataErrorInfo
    {
        #region Prop en field

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;
        [Required(ErrorMessage = "U moet een dag selecteren")]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private DateTime _from;
        [Required(ErrorMessage = "U moet een beginuur selecteren")]
        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }

        private DateTime _until;
        [Required(ErrorMessage = "U moet een einduur selecteren")]
        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }

        private Stage _stage;
        [Required(ErrorMessage = "U moet een stage selecteren")]
        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private Band _band;
        [Required(ErrorMessage = "U moet een band selecteren")]
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

        #region Errorhandling
        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columName
                    });
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return string.Empty;
            }

        }
        #endregion

        #region Enable/Disable Controls
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null),
            null, true);
        }
        #endregion

        #region SQL
        //een line up object maken
        private static LineUp CreateLineUp(DbDataReader reader)
        {
            try
            {
                LineUp lineup = new LineUp();
                lineup.ID = Convert.ToString(reader["lineup_id"]);
                lineup.Date = Convert.ToDateTime(reader["lineup_date"]);
                lineup.From = Convert.ToDateTime(reader["lineup_from"]);
                lineup.Until = Convert.ToDateTime(reader["lineup_until"]);

                lineup.Stage = Stage.GetStageByID(Convert.ToInt16(reader["lineup_stage"]));
                lineup.Band = Band.GetBandByID(Convert.ToInt32(reader["lineup_band"]));
                return lineup;
            }
            catch (Exception ex)
            {
                Console.WriteLine("create lineup: " + ex.Message);
                return null;
            }
        }

        //alle bands ophalen adhv zijn LineUp ID en de line up datum
        public static ObservableCollection<LineUp> GetBandsByLineUpIDAndDate(int id, DateTime date)
        {
            try
            {
                ObservableCollection<LineUp> lstGevondenLineUps = new ObservableCollection<LineUp>();

                string sql = "SELECT * FROM lineup WHERE lineup_stage=@id AND lineup_date=@date ORDER BY lineup_from ASC;";
                DbParameter parID = Database.AddParameter("@id", id);
                DbParameter par1 = Database.AddParameter("@date", date.Date);

                DbDataReader reader = Database.GetData(sql, parID, par1);
                while (reader.Read())
                {
                    lstGevondenLineUps.Add(CreateLineUp(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstGevondenLineUps;
            }
            catch (Exception ex)
            {
                Console.WriteLine("getbands by line up id and date: " + ex.Message);
                return null;
            }
        }

        //alle bands ophalen adhv zijn line up id
        public static ObservableCollection<LineUp> GetBandsByLineUpID(int id)
        {
            try
            {
                ObservableCollection<LineUp> lstGevondenLineUps = new ObservableCollection<LineUp>();
                DbDataReader reader = Database.GetData("SELECT * FROM lineup WHERE lineup_stage = " + id + ";");
                while (reader.Read())
                {
                    lstGevondenLineUps.Add(CreateLineUp(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstGevondenLineUps;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get bands by lineup id: " + ex.Message);
                return null;
            }
        }

        //alle line ups ophalen
        public static ObservableCollection<LineUp> GetLineUps()
        {
            try
            {
                ObservableCollection<LineUp> lstLineUps = new ObservableCollection<LineUp>();
                string sql = "SELECT * FROM lineup";
                DbDataReader reader = Database.GetData(sql);
                while (reader.Read())
                {
                    lstLineUps.Add(CreateLineUp(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstLineUps;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get lineups" + ex.Message);
                return null;
            }
        }

        //1 lineup ophalen adhv zijn ID
        public static LineUp GetLineUpByID(int id)
        {
            try
            {
                LineUp gevondenLineUp = new LineUp();
                string sql = "SELECT * FROM lineup WHERE lineup_id = " + id + "";
                DbDataReader reader = Database.GetData(sql);
                while (reader.Read())
                {
                    gevondenLineUp = CreateLineUp(reader);
                }
                if (reader != null)
                    reader.Close();
                return gevondenLineUp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get line up by id: " + ex.Message);
                return null;
            }
        }

        //een lineup toevoegen aan de DB
        public static void AddLineUp(LineUp lineup)
        {
            try
            {
                string sql = "INSERT INTO lineup(lineup_date, lineup_from, lineup_until, lineup_stage, lineup_band) VALUES (@date, @from, @until, @stageid, @bandid);";

                DbParameter par1 = Database.AddParameter("@date", lineup.Date);
                DbParameter par2 = Database.AddParameter("@from", lineup.From);
                DbParameter par3 = Database.AddParameter("@until", lineup.Until);
                DbParameter par4 = Database.AddParameter("@stageid", lineup.Stage.ID);
                DbParameter par5 = Database.AddParameter("@bandid", lineup.Band.ID);

                int i = Database.ModifyData(sql, par1, par2, par3, par4, par5);
                if (i == 0)
                {
                    MessageBox.Show("Toevoegen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected");
            }
            catch (Exception ex)
            {
                Console.WriteLine("add line up: " + ex.Message);
            }
        }
        #endregion

        #region Methods

        private static ObservableCollection<Band> lstBands = Band.GetBands();

        private static ObservableCollection<FestivalLib.model.LineUp> lstAlleLineUps = GetLineUps();

        //berekend hoeveel tijd er tussen het begin en einde van een optreden zit, dit wordt oa gebruikt voor de breedte van de stackpanel te bepalen
        public static double calculateTimespan(DateTime dtFrom, DateTime dtUntil)
        {
            try
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

                double dTimespan = (dUntil - dFrom) * 100;
                return dTimespan;
            }
            catch (Exception ex)
            {
                Console.WriteLine("calculate timespan" + ex.Message);
                return 150;
            }
        }
        #endregion
    }
}
