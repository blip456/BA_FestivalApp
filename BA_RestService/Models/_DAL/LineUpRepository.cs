using DBHelper;
using FestivalLibPort;
using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using FestivalLib;

namespace BA_RestService.Models._DAL
{
    public class LineUpRepository
    {
        private static LineUp CreateLineUp(DbDataReader reader)
        {
            LineUp lineup = new LineUp();
            lineup.ID = Convert.ToString(reader["lineup_id"]);
            lineup.Date = Convert.ToDateTime(reader["lineup_date"]);
            lineup.From = Convert.ToDateTime(reader["lineup_from"]);
            lineup.Until = Convert.ToDateTime(reader["lineup_until"]);

            lineup.Stage = GetStageByID(Convert.ToInt16(reader["lineup_stage"]));
            lineup.Band = BandRepository.GetBandByID(Convert.ToInt32(reader["lineup_band"]));
            return lineup;
        }

        public static ObservableCollection<LineUp> GetLineUps()
        {
            ObservableCollection<LineUp> lstLineUps = new ObservableCollection<LineUp>();
            string sql = "SELECT * FROM lineup ORDER BY lineup_date";
            DbDataReader reader = Database.GetData(sql);
            while (reader.Read())
            {
                lstLineUps.Add(CreateLineUp(reader));
            }
            if (reader != null)
                reader.Close();
            return lstLineUps;
        }

        public static ObservableCollection<LineUp> GetBandsByDate(DateTime date)
        {
            ObservableCollection<LineUp> lstGevondenLineUps = new ObservableCollection<LineUp>();
            DbDataReader reader = Database.GetData("SELECT * FROM lineup WHERE lineup_date='" + date.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY lineup_from ASC;");

            while (reader.Read())
            {
                lstGevondenLineUps.Add(CreateLineUp(reader));
            }
            if (reader != null)
                reader.Close();
            return lstGevondenLineUps;
        }

        public static Stage GetStageByID(int id)
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
    }
}