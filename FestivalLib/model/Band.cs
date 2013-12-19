using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Band
    {
        #region field en prop
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

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
            gevondenBand = lstAlleBands.Single(i => i.ID == Convert.ToString(id));

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
        #endregion
    }
}
