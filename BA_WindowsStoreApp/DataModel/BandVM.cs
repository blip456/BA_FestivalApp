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

namespace BA_WindowsStoreApp.DataModel
{
    public class BandVM
    {
        public BandVM()
        {
            //Bands = GetBandFromAPI();           
            _test = "tetjesforsure";
        }

        private List<FestivalLibPort.Band> _bands;

        public List<FestivalLibPort.Band> Bands
        {
            get { return _bands; }
            set { _bands = value; }
        }

        private string _test;

        public string Test
        {
            get { return _test; }
            set { _test = value; }
        }
        
        public static List<FestivalLibPort.Band> TestList()
        {
            List<FestivalLibPort.Band> lst = new List<FestivalLibPort.Band>();
            FestivalLibPort.Band testBand = new FestivalLibPort.Band();
            testBand.Name = "testje";
            lst.Add(testBand);
            return lst;
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
