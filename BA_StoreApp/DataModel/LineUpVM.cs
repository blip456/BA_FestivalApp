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
                LineUps = dxml.ReadObject(stream) as List<FestivalLibPort.LineUp>;
            }
        }


    }
}
