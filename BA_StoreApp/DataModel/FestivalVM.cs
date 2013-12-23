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
    public class FestivalVM : ObservableObject
    {
        public FestivalVM()
        {
            GetFestivalFromAPI();
        }

        private Festival _festival;

        public Festival Festival
        {
            get { return _festival; }
            set { _festival = value; OnPropertyChanged("Festival");  }
        }

        private List<DateTime> _dagen;

        public List<DateTime> Dagen
        {
            get { return _dagen; }
            set { _dagen = value; }
        }


        public static List<DateTime> GetDagen(Festival fest)
        {
            List<DateTime> lstDagen = new List<DateTime>();
            TimeSpan timespan = fest.EndDate - fest.StartDate;
            DateTime volgendeDag = fest.StartDate;
            for (int i = 0; i <= timespan.Days + 1; i++)
            {
                TimeSpan ts = TimeSpan.FromDays(i);
                volgendeDag = fest.StartDate.Add(ts);

                lstDagen.Add(volgendeDag.Date);
            }
            return lstDagen;
        }


        public async void GetFestivalFromAPI()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8090/api/festival");
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();

                //DataContractJsonSerializer djs = new
                //DataContractJsonSerializer(typeof(List<FestivalLibPort.Band>));
                //Bands = djs.ReadObject(stream) as List<FestivalLibPort.Band>; 
                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.Festival>));
                Festival = dxml.ReadObject(stream) as FestivalLibPort.Festival;
                Dagen = GetDagen(Festival);
            }
        }
    }
}
