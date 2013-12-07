using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;

namespace Ypsilon2.model
{
    public class Stage
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

        private ObservableCollection<LineUp> _bands;

        public ObservableCollection<LineUp> Bands
        {
            get { return _bands; }
            set { _bands = value; }
        }



        #endregion

        private static ObservableCollection<Stage> lstAlleStages = GetAlleStages();

        private static ObservableCollection<LineUp> lstAlleLineUps = LineUp.GetLineUps();

        #region SQL
        private static Stage CreateStage(DbDataReader reader)
        {
            Stage stage = new Stage();
            stage.ID = Convert.ToString(reader["stage_id"]);
            stage.Name = Convert.ToString(reader["stage_name"]);
            stage.Bands = LineUp.GetBandsByLineUpID(Convert.ToInt32(reader["stage_id"]) );
            return stage;
        }

        private static Stage CreateStageWithDate(DbDataReader reader, DateTime date)
        {
            Stage stage = new Stage();
            stage.ID = Convert.ToString(reader["stage_id"]);
            stage.Name = Convert.ToString(reader["stage_name"]);
            stage.Bands = LineUp.GetBandsByLineUpIDAndDate(Convert.ToInt32(reader["stage_id"]), date);
            return stage;
        }

        public static ObservableCollection<Stage> GetAlleStages()
        {
            ObservableCollection<Stage> lstStages = new ObservableCollection<Stage>();
            DbDataReader reader = Database.GetData("SELECT * FROM stage");
            while (reader.Read())
            {
                lstStages.Add(CreateStage(reader));
            }
            return lstStages;
        }

        public static ObservableCollection<Stage> GetStagesByDay(DateTime date)
        {
            ObservableCollection<Stage> lstStages = new ObservableCollection<Stage>();
            DbDataReader reader = Database.GetData("SELECT * FROM stage");
            while (reader.Read())
            {
                lstStages.Add(CreateStageWithDate(reader, date));
            }
            
            return lstStages;
        }

        public static Stage GetStageByID(int id)
        {
            Stage gevondenStage = new Stage();
            DbDataReader reader = Database.GetData("SELECT * FROM  stage WHERE stage_id = "+id+";");          
            while (reader.Read())
            {
                gevondenStage.Name = (string)reader["stage_name"];                
            }
            return gevondenStage;
        }

        public static void AddStage(string NewPodiumName)
        {
            string sql = "INSERT INTO stage(stage_name) VALUES (@name);";
            DbParameter par1 = Database.AddParameter("@name", NewPodiumName);
            int i = Database.ModifyData(sql, par1);
            Console.WriteLine(i + " row(s) are affected");
        }

        public static void DeleteStage(int id)
        {
            string sql = "DELETE FROM stage WHERE stage_id = @id;";

            DbParameter par1 = Database.AddParameter("@id", id);

            int i = Database.ModifyData(sql, par1);
            Console.WriteLine(i + " row(s) are affected");
        }
        #endregion




    }
}
