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

        #region SQL
        private static Stage CreateStage(DbDataReader reader)
        {
            Stage stage = new Stage();
            stage.ID = Convert.ToString(reader["stage_id"]);
            stage.Name = Convert.ToString(reader["stage_name"]);       
            stage.Bands = LineUp.GetBandsByLineUpID(Convert.ToInt32(reader["stage_id"]));
            return stage;
        }

        public static ObservableCollection<Stage> GetStages()
        {
            ObservableCollection<Stage> lstStages = new ObservableCollection<Stage>();
            DbDataReader reader = Database.GetData("SELECT * FROM stage");
            while (reader.Read())
            {
                lstStages.Add(CreateStage(reader));
            }
            return lstStages;
        } 

        
        #endregion

    }
}
