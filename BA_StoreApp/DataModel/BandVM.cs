using FestivalLibPort;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace BA_StoreApp.DataModel 
{
    public class BandVM : ObservableObject, INotifyPropertyChanged
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

        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
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

        public static async Task<ObservableCollection<Band>> GetBandsAsync()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:8081/api/band");
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                ObservableCollection<Band> lst = new ObservableCollection<Band>();
                //DataContractJsonSerializer djs = new
                //DataContractJsonSerializer(typeof(List<FestivalLibPort.Band>));
                //Bands = djs.ReadObject(stream) as List<FestivalLibPort.Band>; 
                DataContractSerializer dxml = new DataContractSerializer(typeof(ObservableCollection<FestivalLibPort.Band>));
                lst = dxml.ReadObject(stream) as ObservableCollection<FestivalLibPort.Band>;
                return lst;
            }

            return null;
        }

        public static Band GetBandAsync2(string selectedBand)
        {
            Band gevondenBand = new Band();
            //ObservableCollection<Band> lst = await GetBandsAsync();
            
            return gevondenBand;
        }

        public static async Task<Band> GetBandAsync(string selectedBand)
        {
            ObservableCollection<Band> lst = await GetBandsAsync();
            Band gevondenband = new Band();
            gevondenband = lst.Single(item => item.ID == selectedBand);

            return gevondenband ;
        }

       
    }
}
