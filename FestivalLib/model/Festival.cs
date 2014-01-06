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
        [Required(ErrorMessage = "U moet een begindatum selecteren")]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }


        private DateTime _endDatde;
        [Required(ErrorMessage = "U moet een einddatum selecteren")]
        public DateTime EndDate
        {
            get { return _endDatde; }
            set { _endDatde = value; }
        }

        private string _omschrijving;
        [Required(ErrorMessage = "U moet een omschrijving invullen")]
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
        //alle festivallen ophalen uit de DB (aanmaken van festival object zit hier al in)
        public static Festival GetFestivals()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("get festivals: " + ex.Message);
                return null;
            }
        }

        //festival aanpassen
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
                Console.WriteLine("Edit festival: " + ex.Message);
            }
        }

        //deze methode zal tickettypes toevoegen aan de DB indien de datum van het festival verlengd wordt
        //er wordt telkens gekenen of het tickettype (vip en normal) voor de dag al bestaat, indien deze niet bestaat zal DAGx VIP of DAGx Normaal toegevoegd worden
        //er zullen standaardwaarden ingevuld worden
        public static void ChangeTicketDates()
        {
            try
            {
                ObservableCollection<DateTime> dagen = aantalDagen();
                int iName = 1;
                bool isVIP = false;

                for (int i = 0; i < dagen.Count * 2; i++)
                {
                    string sql = "INSERT INTO tickettype (tickettype_name, tickettype_price, tickettype_categorie, tickettype_available) SELECT @name, @price, @cat, @avail WHERE NOT EXISTS (SELECT 1 FROM tickettype WHERE tickettype_name = @name AND tickettype_categorie = @cat);";

                    DbParameter par1 = Database.AddParameter("@name", "Dag " + iName);
                    DbParameter par2 = Database.AddParameter("@cat", "");
                    if (isVIP == false)
                    {
                        par2 = Database.AddParameter("@cat", "Normaal");
                        isVIP = true;
                    }
                    else if (isVIP == true)
                    {
                        par2 = Database.AddParameter("@cat", "VIP");
                        iName += 1;
                        isVIP = false;
                    }
                    DbParameter par3 = Database.AddParameter("@price", 0);
                    DbParameter par4 = Database.AddParameter("@avail", 0);
                    int iResult = Database.ModifyData(sql, par1, par2, par3, par4);
                    Console.WriteLine(iName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Change ticket dates: " + ex.Message);
            }
        }
        #endregion

        #region Public Vars
        //deze berekend hoeveel dagen er tussen de begintijd en den eindtijd zit en returned een lijst met date times
        public static ObservableCollection<DateTime> aantalDagen()
        {
            try
            {
                ObservableCollection<DateTime> lstDagen = new ObservableCollection<DateTime>();
                Festival festival = GetFestivals();
                TimeSpan timespan = festival.EndDate - festival.StartDate;
                DateTime volgendeDag = festival.StartDate;
                for (int i = 0; i <= timespan.Days; i++)
                {
                    TimeSpan ts = TimeSpan.FromDays(i);
                    volgendeDag = festival.StartDate.Add(ts);

                    lstDagen.Add(volgendeDag.Date);
                }
                return lstDagen;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Aantal dagen: " + ex.Message);
                return null;
            }
        }
        #endregion
    }
}
