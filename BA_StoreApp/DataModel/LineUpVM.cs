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
    public class LineUpVM:ObservableObject
    {
        public LineUpVM()
        {
            GetLineUpsFromAPI();
        }
        
        private List<LineUp> _lineUps;

        public List<LineUp> LineUps
        {
            get { return _lineUps; }
            set { _lineUps = value; OnPropertyChanged("LineUps"); }
        }

        private List<Stage> _uniekeStages;

        public List<Stage> UniekeStages
        {
            get { return _uniekeStages; }
            set { _uniekeStages = value; OnPropertyChanged("UniekeStages"); }
        }

        private LineUp _selectedLineUp;

        public LineUp SelectedLineUp
        {
            get { return _selectedLineUp; }
            set { _selectedLineUp = value; OnPropertyChanged("SelectedLineUp"); }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set 
            { 
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
                GetLineUpsByDateFromAPI(_selectedDate);
            }
        }
               
        public async void GetLineUpsFromAPI()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8090/api/lineup");
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.LineUp>));
                List<LineUp> lst = dxml.ReadObject(stream) as List<FestivalLibPort.LineUp>;
                lst.OrderBy(item => item.Stage.ID).ThenBy(item => item.From);
                LineUps = lst;
            }
        }

        public async void GetLineUpsByDateFromAPI(DateTime date)
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            string sDate = date.Day + "-" + date.Month + "-" + date.Year;

            HttpResponseMessage response = await client.GetAsync("http://localhost:8090/api/lineup/"+sDate);
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.LineUp>));
                List<LineUp>lst = dxml.ReadObject(stream) as List<FestivalLibPort.LineUp>;
                lst.OrderBy(item => item.Stage.ID).ThenBy(item=>item.From);
                LineUps = lst;
            }
        }
    }
}
