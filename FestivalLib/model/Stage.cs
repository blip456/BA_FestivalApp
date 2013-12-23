﻿using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
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
            if (reader != null)
                reader.Close();
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
            if (reader != null)
                reader.Close();
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
            if (reader != null)
                reader.Close();
            return gevondenStage;
        }

        public static void AddStage(string NewPodiumName)
        {
            try
            {
                string sql = "INSERT INTO stage(stage_name) VALUES (@name);";
                DbParameter par1 = Database.AddParameter("@name", NewPodiumName);
                int i = Database.ModifyData(sql, par1);
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

        public static void DeleteStage(int id)
        {
            try
            {
                string sql = "DELETE FROM stage WHERE stage_id = @id;";

                DbParameter par1 = Database.AddParameter("@id", id);

                int i = Database.ModifyData(sql, par1);
                if (i == 0)
                {
                    MessageBox.Show("Verwijderen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected");
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
