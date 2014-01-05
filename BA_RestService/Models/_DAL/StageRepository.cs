using DBHelper;
using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace BA_RestService.Models._DAL
{
    public class StageRepository
    {
        private static Stage CreateStageWithDate(DbDataReader reader, DateTime date)
        {
            try
            {
                Stage stage = new Stage();
                stage.ID = Convert.ToString(reader["stage_id"]);
                stage.Name = Convert.ToString(reader["stage_name"]);
                stage.Bands = GetBandsByLineUpIDAndDate(Convert.ToInt32(reader["stage_id"]), date);
                return stage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("create stage with date: " + ex.Message);
                return null;
            }
        }

        private static LineUp CreateLineUp(DbDataReader reader)
        {
            try
            {
                LineUp lineup = new LineUp();
                lineup.ID = Convert.ToString(reader["lineup_id"]);
                lineup.Date = Convert.ToDateTime(reader["lineup_date"]);
                lineup.From = Convert.ToDateTime(reader["lineup_from"]);
                lineup.Until = Convert.ToDateTime(reader["lineup_until"]);

                lineup.Stage = GetStageByID(Convert.ToInt16(reader["lineup_stage"]));
                lineup.Band = GetBandByID(Convert.ToInt32(reader["lineup_band"]));
                return lineup;
            }
            catch (Exception ex)
            {
                Console.WriteLine("create lineup: " + ex.Message);
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
                band.Genres = GetGenresByBandID(Convert.ToInt32(reader["band_id"]));
                return band;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create band: " + ex.Message);
                return null;
            }
        }

        public static List<Genre> GetGenresByBandID(int id)
        {
            try
            {
                string query = "SELECT * FROM genre INNER JOIN bandgenre on bandgenre.genre_id = genre.genre_id WHERE band_id = " + id + ";";

                List<Genre> gevondenGenres = new List<Genre>();
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

        public static Stage GetStageByID(int id)
        {
            try
            {
                Stage gevondenStage = new Stage();
                DbDataReader reader = Database.GetData("SELECT * FROM  stage WHERE stage_id = " + id + ";");
                while (reader.Read())
                {
                    gevondenStage.Name = (string)reader["stage_name"];
                }
                if (reader != null)
                    reader.Close();
                return gevondenStage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("getstages by id" + ex.Message);
                return null;
            }
        }

        public static List<LineUp> GetBandsByLineUpIDAndDate(int id, DateTime date)
        {
            try
            {
                List<LineUp> lstGevondenLineUps = new List<LineUp>();

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

        public static List<Stage> GetStagesByDay(DateTime date)
        {
            try
            {
                List<Stage> lstStages = new List<Stage>();
                DbDataReader reader = Database.GetData("SELECT * FROM stage");
                while (reader.Read())
                {
                    lstStages.Add(CreateStageWithDate(reader, date));
                }
                if (reader != null)
                    reader.Close();
                return lstStages;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get all stages by day: " + ex.Message);
                return null;
            }
        }
    }
}