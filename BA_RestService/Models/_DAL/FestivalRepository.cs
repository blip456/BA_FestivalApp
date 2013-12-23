using DBHelper;
using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace BA_RestService.Models._DAL
{
    public class FestivalRepository
    {
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
    }
}