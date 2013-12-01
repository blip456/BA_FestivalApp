using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ypsilon2.model;

namespace Ypsilon2.viewmodel
{
    class BandsVM:ObservableObject, IPage
    {
        #region fields en props

        public string Name
        {
            get { return "Bands"; }
        }

        public BandsVM()
        {
            _bands = Band.GetBands();
            _gefilterdeBands = Bands;
        }

        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        private ObservableCollection<Band> _gefilterdeBands;

        public ObservableCollection<Band> GefilterdeBands
        {
            get { return _gefilterdeBands; }
            set { _gefilterdeBands = value; OnPropertyChanged("GefilterdeBands"); }
        }

        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
        }

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; OnPropertyChanged("SearchString"); }
        }
        
        #endregion 

        #region commands
        public ICommand SearchCommand
        {
            get { return new RelayCommand<string>(Search); }
        }

        private void Search(string str)
        {
            Console.WriteLine(str);
            GefilterdeBands = model.Band.GetBandsByString(Bands, str);
        }


        public ICommand SaveBandCommand
        {
            get { return new RelayCommand<Band>(SaveBand); }
        }

        public void SaveBand(Band band)
        {
            Band.EditBand(band);
            GefilterdeBands = Band.GetBands();
        }

        public ICommand AddBandCommand
        {
            get { return new RelayCommand<string>(AddBand); }
        }
        public void AddBand(string strings)
        {
            
            Band band = new Band();
            band.ID = "20";
            band.Twitter = "test";
            band.Facebook = "test";
            band.Descr = "test";
            band.Picture = "no pic";
            band.Name = strings;
            Band.AddBand(band);
            GefilterdeBands =Band.GetBands();
            
        }

        public ICommand DeleteBandCommand
        {
            get { return new RelayCommand<int>(DeleteBand); }
        }

        public void DeleteBand(int id)
        {
            //Een bevestiging of de gebruiker wel degelijk deze contactpersoon wilt verwijderen
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Band.GetBandByID(Bands, id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                Band.DeleteBand(id);
                GefilterdeBands = Band.GetBands();
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}
