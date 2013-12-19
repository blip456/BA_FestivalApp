using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BA_WindowsStoreApp.DataModel;



namespace BA_WindowsStoreApp.DataModel 
{
    public class BandVM:ObservableObject
    {
        public BandVM()
        {
            GetBandFromAPI();           
        }

        private List<FestivalLibPort.Band> _bands;

        public List<FestivalLibPort.Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }       

        public async void GetBandFromAPI()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/band");
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();

                //DataContractJsonSerializer djs = new
                //DataContractJsonSerializer(typeof(List<FestivalLibPort.Band>));
                //Bands = djs.ReadObject(stream) as List<FestivalLibPort.Band>; 
                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.Band>));
                Bands = dxml.ReadObject(stream) as List<FestivalLibPort.Band>;      
            }            
        }

        public async void GetBands()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/band");
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();

                //DataContractJsonSerializer djs = new
                //DataContractJsonSerializer(typeof(List<FestivalLibPort.Band>));
                //Bands = djs.ReadObject(stream) as List<FestivalLibPort.Band>; 

                DataContractSerializer dxml = new DataContractSerializer(typeof(List<FestivalLibPort.Band>));
                Bands = dxml.ReadObject(stream) as List<FestivalLibPort.Band>;
            }
        }

    }
}
