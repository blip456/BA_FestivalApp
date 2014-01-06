using DBHelper;
using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace BA_RestService.Models._DAL
{
    public class GenreRepository
    {
        public static List<Genre> Getgenres()
        {
            try
            {
                string sql = "SELECT * FROM genre";
                List<Genre> lst = new List<Genre>();
                DbDataReader reader = Database.GetData(sql);
                while (reader.Read())
                {
                    Genre genre = new Genre();
                    genre.Name = Convert.ToString(reader["genre_name"]);
                    genre.ID = Convert.ToString(reader["genre_id"]);
                    genre.Bands = GetBandByGenreId((int)reader["genre_id"]);
                    lst.Add(genre);
                }
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Band> GetBandByGenreId(int id)
        {
            try
            {
                string sql = "SELECT * FROM bandgenre WHERE genre_id=@id";
                DbParameter parID = Database.AddParameter("@id", id);
                DbDataReader reader = Database.GetData(sql, parID);
                List<Band> lst = new List<Band>();
                while (reader.Read())
                {
                    Band gevondenBand = GetBandByID((int)reader["band_id"]);
                    lst.Add(gevondenBand);                       
                }
                return lst;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

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
                band.Genres = null;
                return band;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}