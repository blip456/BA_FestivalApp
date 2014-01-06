using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Band : IDataErrorInfo
    {
        #region Field en Prop
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage = "U moet een naam invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een naam moet tussen de 3 en 50 karakters liggen")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private byte[] _picture;

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private string _descr;
        [Required(ErrorMessage = "U moet een beschrijving invullen")]
        [StringLength(255, MinimumLength=5, ErrorMessage="Een omschrijving moet tussen de 5 en 255 karakters liggen")]
        public string Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }

        private string _twitter;

        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private string _facebook;

        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
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
        //aanmaken van een band object
        private static Band CreateBand(DbDataReader reader)
        {
            try
            {
                Band band = new Band();
                band.ID = Convert.ToString(reader["band_id"]);
                band.Name = Convert.ToString(reader["band_name"]);
                band.Picture = (byte[])reader["band_picture"];
                band.Descr = Convert.ToString(reader["band_description"]);
                band.Twitter = Convert.ToString(reader["band_twitter"]);
                band.Facebook = Convert.ToString(reader["band_facebook"]);
                band.Genres = Genre.GetGenresByBandID(Convert.ToInt32(reader["band_id"]));
                return band;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create band: " + ex.Message);
                return null;
            }
        }

        //alle bands ophalen uit de db
        public static ObservableCollection<Band> GetBands()
        {
            try
            {
                ObservableCollection<Band> lstBands = new ObservableCollection<Band>();
                DbDataReader reader = Database.GetData("SELECT * FROM band");
                while (reader.Read())
                {
                    lstBands.Add(CreateBand(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstBands;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Bands: " + ex.Message);
                return null;
            }
        }

        //alle bands oophalen die aan een bepaalde string voldoen in hun band name
        public static ObservableCollection<Band> GetBandsByString(string search)
        {
            try
            {
                ObservableCollection<Band> lstGevondenBands = new ObservableCollection<Band>();
                foreach (Band band in lstAlleBands)
                {
                    if (band.Name.ToUpper().Contains(search.ToUpper()) || band.Descr.ToUpper().Contains(search.ToUpper()) || band.Facebook.ToUpper().Contains(search.ToUpper()) || band.Twitter.ToUpper().Contains(search.ToUpper()))
                    {
                        lstGevondenBands.Add(band);
                    }
                }
                return lstGevondenBands;
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                return null;
            }
        }

        //1 band ophalen adhv een band id
        public static Band GetBandByID(int id)
        {
            try
            {
                Band gevondenBand = new Band();

                string sql = "SELECT * FROM band WHERE band_id=@id";
                DbParameter parID = Database.AddParameter("@id", id);
                DbDataReader reader = Database.GetData(sql, parID);
                while (reader.Read())
                {
                    gevondenBand = CreateBand(reader);
                }
                if (reader != null)
                    reader.Close();

                return gevondenBand;
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                return null;
            }
        }

        //1 band ophalen adhv een bepaalde string die moet voldoen in hun band name
        public static Band GetBandByString(string bandName)
        {
            try
            {
                Band gevondenBand = new Band();
                string sql = "SELECT * FROM band WHERE band_name=@name";
                DbParameter par1 = Database.AddParameter("@name", bandName);
                DbDataReader reader = Database.GetData(sql, par1);
                while (reader.Read())
                {
                    gevondenBand = CreateBand(reader);
                }
                if (reader != null)
                    reader.Close();
                return gevondenBand;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get band by string: " + ex.Message);
                return null;
            }
        }

        //1 band toevoegen aan de DB
        public static void AddBand(Band band)
        {
            try
            {
                string sql = "INSERT INTO band(band_name, band_picture, band_description, band_twitter, band_facebook) VALUES (@name, @picture, @description, @twitter, @facebook);";

                DbParameter par1 = Database.AddParameter("@name", band.Name);
                DbParameter par2 = Database.AddParameter("@picture", band.Picture);
                DbParameter par3 = Database.AddParameter("@description", band.Descr);
                DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
                DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);

                int i = Database.ModifyData(sql, par1, par2, par3, par4, par5);
                if (i == 0)
                {
                    MessageBox.Show("Toevoegen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected");

                Band newBand = GetBandByString(band.Name);
                //toevoegen van genre
                foreach (Genre genre in band.Genres)
                {
                    Genre.AddGenre(genre, newBand);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add band:" + ex.Message);
            }

            lstAlleBands = GetBands();
        }

        //1 band aanpassen in de DB
        public static void EditBand(Band band)
        {
            try
            {
                //updaten van gewone band gegevens excl genre
                string sql = "UPDATE band SET band_name=@name, band_picture=@picture, band_description=@description, band_twitter=@twitter, band_facebook=@facebook WHERE band_id=@ID;";

                DbParameter par1 = Database.AddParameter("@name", band.Name);
                DbParameter par2 = Database.AddParameter("@picture", band.Picture);
                DbParameter par3 = Database.AddParameter("@description", band.Descr);
                DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
                DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);

                DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(band.ID));

                int i = Database.ModifyData(sql, par1, par2, par3, par4, par5, parID);
                if (i == 0)
                {
                    MessageBox.Show("Opslaan mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected");

                //updaten van genre
                foreach (Genre genre in band.Genres)
                {
                    Genre.AddGenre(genre, band);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Edit Band: " + ex.Message);
            }

            lstAlleBands = GetBands();
        }

        //1 band verwijderen in de DB
        public static void DeleteBand(int id)
        {
            try
            {
                string sql = "DELETE FROM band WHERE band_id = @ID;";

                DbParameter parID = Database.AddParameter("@ID", id);

                int i = Database.ModifyData(sql, parID);
                if (i == 0)
                {
                    MessageBox.Show("Verwijderen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delet band" + ex.Message);
            }
        }

        //1 band verwijderen uit een specifieke line up
        public static void DeleteBandFromLineUp(int lineupID)
        {
            try
            {
                string sql = "DELETE FROM lineup WHERE lineup_id = @lineupID;";
                DbParameter parLineUpID = Database.AddParameter("@lineupID", lineupID);
                int i = Database.ModifyData(sql, parLineUpID);
                if (i == 0)
                {
                    MessageBox.Show("Verwijderen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + "row deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete band from lineup" + ex.Message);
            }
        }

        //kijken of een band nog in een bepaalde line up zit indien ja kun je de band niet verwijderen
        public static bool IsBandDeleteAllowed(int bandID)
        {
            try
            {
                bool isDeleteAllowed = false;
                int i = 0;
                string sql = "SELECT * FROM lineup WHERE lineup_band=@ID;";
                DbParameter parID = Database.AddParameter("@ID", bandID);
                DbDataReader reader = Database.GetData(sql, parID);
                while (reader.Read())
                {
                    i += 1;
                }
                if (i >= 1)
                {
                    isDeleteAllowed = false;
                }
                else if (i == 0)
                {
                    isDeleteAllowed = true;
                }
                if (reader != null)
                    reader.Close();
                return isDeleteAllowed;
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsBandDeleteAllowed: " + ex.Message);
                return false;
            }

        }

        //1 band verwijderen uit de DB
        public static void DeleteGenre(Band selectedBand, Genre selectedGenre)
        {
            try
            {
                string sql = "DELETE FROM bandgenre WHERE band_id=@bandid AND genre_id=@genreid;";
                DbParameter par1 = Database.AddParameter("@bandid", selectedBand.ID);
                DbParameter par2 = Database.AddParameter("@genreid", selectedGenre.ID);
                int i = Database.ModifyData(sql, par1, par2);
                Console.WriteLine(i + " rows deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete band:" + ex.Message);
            }
        }
        #endregion

        #region Public Vars
        public static ObservableCollection<Band> lstAlleBands = GetBands();
        #endregion
    }
}
