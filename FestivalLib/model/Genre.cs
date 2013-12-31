using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using Xceed.Wpf.Toolkit;
//using FestivalLib.Model;

namespace FestivalLib.model
{
    public class Genre
    {
        #region field en properties

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


        #endregion

        #region SQL
        public static ObservableCollection<Genre> GetGenresByBandID(int id)
        {
            string query = "SELECT * FROM genre INNER JOIN bandgenre on bandgenre.genre_id = genre.genre_id WHERE band_id = " + id + ";";

            ObservableCollection<Genre> gevondenGenres = new ObservableCollection<Genre>();
            DbDataReader reader = Database.GetData(query);
            while (reader.Read())
            {
                Genre genre = new Genre();
                genre.Name = Convert.ToString(reader["genre_name"]);
                genre.ID = Convert.ToString(reader["genre_id"]);
                gevondenGenres.Add(genre);
            }
            if (reader != null)
                reader.Close();
            return gevondenGenres;
        }

        public static ObservableCollection<Genre> Getgenres()
        {
            string sql = "SELECT * FROM genre";
            ObservableCollection<Genre> lst = new ObservableCollection<Genre>();
            DbDataReader reader = Database.GetData(sql);
            while (reader.Read())
            {
                Genre genre = new Genre();
                genre.Name = Convert.ToString(reader["genre_name"]);
                genre.ID = Convert.ToString(reader["genre_id"]);
                lst.Add(genre);
            }
            return lst;
        }


        public static Genre GetGenreByString(string genre)
        {
            string sql = "SELECT * FROM genre WHERE genre_name=@name;";
            DbParameter par1 = Database.AddParameter("@name", genre);
            DbDataReader reader = Database.GetData(sql, par1);
            Genre gevondenGenre = new Genre();
            while (reader.Read())
            {
                gevondenGenre.Name = Convert.ToString(reader["genre_name"]);
                gevondenGenre.ID = Convert.ToString(reader["genre_id"]);
            }
            return gevondenGenre;
        }

        //public static void AddGenre(Band band)
        //{
        //    try
        //    {
        //        string sql = "INSERT INTO genre(genre_name) values (@name);";

        //        DbParameter par1 = Database.AddParameter("@name", sGenreNaam);

        //        int i = Database.ModifyData(sql, par1);
        //        if (i == 0)
        //        {
        //            MessageBox.Show("Toevoegen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
        //        }
        //        Console.WriteLine(i + " row(s) are affected");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw ex;
        //    }
        //}

        //public static void AddGenreToBand(Band band)
        //{

        //}
        #endregion
    }
}
