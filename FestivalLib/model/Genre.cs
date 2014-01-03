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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("get genres by band id: " + ex.Message);
                return null;
            }
        }

        public static ObservableCollection<Genre> Getgenres()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("get genres: " + ex.Message);
                return null;
            }
        }

        public static void AddGenre(Genre genre, Band band)
        {
            try
            {
                //genre koppelen aan de band
                string sql2 = "INSERT INTO bandgenre (band_id, genre_id) VALUES(@band, @genre);";
                DbParameter parBand = Database.AddParameter("@band", band.ID);
                DbParameter parGenre = Database.AddParameter("@genre", genre.ID);
                Database.ModifyData(sql2, parBand, parGenre);
            }
            catch (Exception ex)
            {
                Console.WriteLine("add genre" + ex.Message);
            }
        }

        public static void AddGenreToDB(string genreName)
        {
            try
            {
                //toevoegen van nieuw genre aan db
                string sql = "INSERT INTO genre (genre_name) VALUES (@name);";
                DbParameter par1 = Database.AddParameter("@name", genreName);
                Database.ModifyData(sql, par1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
            }
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
        #endregion
    }
}
