using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Festival : IDataErrorInfo
    {
        #region Field en properties

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        

        private string _name;
        [Required(ErrorMessage = "U moet een naam invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een naam moet tussen de 3 en 50 karakters liggen")] 
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }        

        private DateTime _startDate;
        [Required(ErrorMessage="U moet een begindatum selecteren")]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }


        private DateTime _endDatde;
        [Required(ErrorMessage="U moet een einddatum selecteren")]
        public DateTime EndDate
        {
            get { return _endDatde; }
            set { _endDatde = value; }
        }

        private string _omschrijving;
        [Required(ErrorMessage="U moet een omschrijving invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een omschrijving moet tussen de 20 en 255 karakters liggen")] 
        public string Omschrijving
        {
            get { return _omschrijving; }
            set { _omschrijving = value; }
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
            if (reader != null)
                reader.Close();
            return festival;
        }       

        public static void EditFestival(Festival festival)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
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
