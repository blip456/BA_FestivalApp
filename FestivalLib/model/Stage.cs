using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Stage : IDataErrorInfo
    {
        #region field en properties
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage = "U moet een naam invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een naam moet tussen de 3 en 50 karakters liggen")]
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

        #region Errorhandling
        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columName
                    });
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return string.Empty;
            }

        }
        #endregion

        #region Enable/Disable Controls
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null),
            null, true);
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
            stage.Bands = LineUp.GetBandsByLineUpID(Convert.ToInt32(reader["stage_id"]));
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
            DbDataReader reader = Database.GetData("SELECT * FROM  stage WHERE stage_id = " + id + ";");
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

        public static void EditStage(string NewPodiumName, Stage SelectedStageEdit)
        {
            string sql = "UPDATE stage SET stage_name=@name WHERE stage_id=@id;";
            DbParameter parID = Database.AddParameter("@id", SelectedStageEdit.ID);
            DbParameter par1 = Database.AddParameter("@name", NewPodiumName);

            int i = Database.ModifyData(sql, par1, parID);
            if (i == 0)
            {
                MessageBox.Show("Aanpassen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
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

        public static bool IsStageDeleteAllowed(int stageID)
        {
            bool isDeleteAllowed = false;
            int i = 0;
            string sql = "SELECT * FROM lineup WHERE lineup_stage=@ID;";
            DbParameter parID = Database.AddParameter("@ID", stageID);
            DbDataReader reader = Database.GetData(sql, parID);
            while (reader.Read())
            {
                i += 1;
            }
            if (i >= 1)
            {
                isDeleteAllowed = false;
            }
            else if (i == 0)
            {
                isDeleteAllowed = true;
            }
            if (reader != null)
                reader.Close();
            return isDeleteAllowed;
        }

        #endregion
    }
}
