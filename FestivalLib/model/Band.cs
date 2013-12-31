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
        #region field en prop
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
        [RegularExpression(@"^.{20,255}$")]
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

        public static ObservableCollection<Band> lstAlleBands = GetBands();

        #region SQL
        private static Band CreateBand(DbDataReader reader)
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

        public static ObservableCollection<Band> GetBands()
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

        public static ObservableCollection<Band> GetBandsByString(string search)
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

        public static Band GetBandByID(int id)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            lstAlleBands = GetBands();
        }

        public static void EditBand(Band band)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            lstAlleBands = GetBands();
        }

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
                Console.WriteLine(ex.Message);
                throw ex;
            }


        }

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
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static bool IsBandDeleteAllowed(int bandID)
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

        public static void AddGenre(Band SelectedBand, string genre)
        {
            if (SelectedBand.Genres.Count == 0)
            {
                if (Genre.Getgenres().Any(item => item.Name == genre))
                {
                    //zoeken van nieuw genre adhv string en dan koppelen aan band
                    Genre gevondenGenre = Genre.GetGenreByString(genre);

                    //genre koppelen aan de band
                    //string sql2 = "UPDATE bandgenre SET band_id=@band, genre_id=@genre;";
                    string sql2 = "INSERT INTO bandgenre (band_id, genre_id) VALUES(@band, @genre);";
                    DbParameter parBand = Database.AddParameter("@band", SelectedBand.ID);
                    DbParameter parGenre = Database.AddParameter("@genre", gevondenGenre.ID);
                    Database.ModifyData(sql2, parBand, parGenre);


                }
                else
                {
                    //toevoegen van nieuw genre aan db
                    Genre newGenre = new Genre();
                    newGenre.Name = genre;
                    string sql = "INSERT INTO genre (genre_name) VALUES (@name);";
                    DbParameter par1 = Database.AddParameter("@name", genre);
                    Database.ModifyData(sql, par1);

                    //zoeken van nieuw genre adhv string en dan koppelen aan band
                    Genre gevondenGenre = Genre.GetGenreByString(genre);

                    //genre koppelen aan de band
                    //string sql2 = "UPDATE bandgenre SET band_id=@band, genre_id=@genre;";
                    string sql2 = "INSERT INTO bandgenre (band_id, genre_id) VALUES(@band, @genre);";
                    DbParameter parBand = Database.AddParameter("@band", SelectedBand.ID);
                    DbParameter parGenre = Database.AddParameter("@genre", gevondenGenre.ID);
                    Database.ModifyData(sql2, parBand, parGenre);
                }
            }

            else
            {
                foreach (Genre oGenre in SelectedBand.Genres)
                {
                    if (oGenre.Name != genre)
                    {
                        if (Genre.Getgenres().Any(item => item.Name == genre))
                        {
                            //zoeken van nieuw genre adhv string en dan koppelen aan band
                            Genre gevondenGenre = Genre.GetGenreByString(genre);

                            //genre koppelen aan de band
                            //string sql2 = "UPDATE bandgenre SET band_id=@band, genre_id=@genre;";
                            string sql2 = "INSERT INTO bandgenre (band_id, genre_id) VALUES(@band, @genre);";
                            DbParameter parBand = Database.AddParameter("@band", SelectedBand.ID);
                            DbParameter parGenre = Database.AddParameter("@genre", gevondenGenre.ID);
                            Database.ModifyData(sql2, parBand, parGenre);

                            break;
                        }
                        else
                        {
                            //toevoegen van nieuw genre aan db
                            Genre newGenre = new Genre();
                            newGenre.Name = genre;
                            string sql = "INSERT INTO genre (genre_name) VALUES (@name);";
                            DbParameter par1 = Database.AddParameter("@name", genre);
                            Database.ModifyData(sql, par1);

                            //zoeken van nieuw genre adhv string en dan koppelen aan band
                            Genre gevondenGenre = Genre.GetGenreByString(genre);

                            //genre koppelen aan de band
                            //string sql2 = "UPDATE bandgenre SET band_id=@band, genre_id=@genre;";
                            string sql2 = "INSERT INTO bandgenre (band_id, genre_id) VALUES(@band, @genre);";
                            DbParameter parBand = Database.AddParameter("@band", SelectedBand.ID);
                            DbParameter parGenre = Database.AddParameter("@genre", gevondenGenre.ID);
                            Database.ModifyData(sql2, parBand, parGenre);

                            break;
                        }

                    }
                }


            }

        }
        #endregion


    }
}
