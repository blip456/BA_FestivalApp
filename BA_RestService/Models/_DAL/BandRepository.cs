using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BA_RestService.Models;
using System.Data.Common;
using DBHelper;

namespace BA_RestService.Models._DAL
{
    public class BandRepository
    {        
        private static Band CreateBand(DbDataReader reader)
        {

            Band band = new Band();
            band.ID = Convert.ToString(reader["band_id"]);
            band.Name = Convert.ToString(reader["band_name"]);
            band.Picture = (byte[])reader["band_picture"];
            band.Descr = Convert.ToString(reader["band_description"]);
            band.Twitter = Convert.ToString(reader["band_twitter"]);
            band.Facebook = Convert.ToString(reader["band_facebook"]);
            //band.Genres = Genre.GetGenresByBandID(Convert.ToInt32(reader["band_id"]));
            return band;
        }

        public static List<Band> GetBands()
        {
            List<Band> lstBands = new List<Band>();
            DbDataReader reader = Database.GetData("SELECT * FROM band");
            while (reader.Read())
            {
                lstBands.Add(CreateBand(reader));
            }
            return lstBands;
        }

        public static Band GetBandByID(int id)
        {
            Band gevondenBand = new Band();
            string sql = "SELECT * FROM band WHERE band_id=@id;";
            DbParameter parID = Database.AddParameter("@id", id);
            DbDataReader reader = Database.GetData(sql, parID);           
            while (reader.Read())
            {
                gevondenBand = CreateBand(reader);
            }            
            return gevondenBand;
        }
    }
}