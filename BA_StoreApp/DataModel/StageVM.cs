using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BA_StoreApp.DataModel
{
    public class StageVM:ObservableObject
    {
        private List<Stage> _stages;

        public List<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
                GetStagesByDateFromAPI(_selectedDate);
            }
        }

        public async void GetStagesByDateFromAPI(DateTime date)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            string sDate = date.Day + "-" + date.Month + "-" + date.Year;
            HttpResponseMessage response = await client.GetAsync("http://localhost:8090/api/stage/"+sDate);
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.Stage>));
                Stages = dxml.ReadObject(stream) as List< FestivalLibPort.Stage>;                
            }
        }
        
    }
}
