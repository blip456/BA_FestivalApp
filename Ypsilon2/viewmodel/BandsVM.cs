using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FestivalLib;
using System.IO;
using FestivalLib.model;

namespace Ypsilon2.viewmodel
{
    class BandsVM : ObservableObject, IPage
    {
        #region fields en props

        public BandsVM()
        {
            _bands = Band.GetBands();
            _gefilterdeBands = Bands;
            _imageSource = convertStringToByte("../../content/images/blank.jpg");
            _band = new Band();
            _band.Genres = new ObservableCollection<Genre>();
            //_selectedBand = new Band();
        }

        public string Name
        {
            get { return "Bands"; }
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
            set
            {
                _selectedBand = value;

                //als er een band geselecteerd is zal de foto van die band getoond worden, indien er geen foto in de DB zit zal een 'blank' foto getoond worden
                //als er een band geselecteerd is zal het huidige contact die contactpersoon zijn
                if (SelectedBand != null)
                {
                    Band = SelectedBand;
                    if (SelectedBand.Picture != null && SelectedBand.Picture.Length > 6)
                    {
                        ImageSource = SelectedBand.Picture;
                    }
                    else
                    {
                        ImageSource = convertStringToByte("../../content/images/blank.jpg");
                    }
                }
                Band = _selectedBand;
                OnPropertyChanged("SelectedBand");

            }
        }

        private Band _band;

        public Band Band
        {
            get { return _band; }
            set { _band = value; OnPropertyChanged("Band"); }
        }

        private byte[] _imageSource;

        public byte[] ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged("ImageSource"); }
        }

        private string _selectedImagePad;

        public string SelectedImagePad
        {
            get { return _selectedImagePad; }
            set { _selectedImagePad = value; }
        }

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; OnPropertyChanged("SearchString"); }
        }

        private string _genre;

        public string Genre
        {
            get { return _genre; }
            set { _genre = value; OnPropertyChanged("Genre"); }
        }

        private Genre _selectedGenre;

        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; OnPropertyChanged("SelectedGenre"); }
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
            GefilterdeBands = FestivalLib.model.Band.GetBandsByString(str);
        }

        public ICommand SelectImageCommand
        {
            get { return new RelayCommand(SelectImage); }
        }
        private void SelectImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPG(*.jpg)|*.jpg|PNG(*.png)|*.png";
            ofd.Title = "Selecteer een afbeelding";
            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                SelectedImagePad = ofd.FileName.ToString();
                ImageSource = convertStringToByte(SelectedImagePad);
            }
        }

        public ICommand SaveBandCommand
        {
            get { return new RelayCommand(SaveBand, Band.IsValid); }
        }
        public void SaveBand()
        {
            Band.Picture = ImageSource;
            //add genre to band oproepen

            Band.EditBand(Band);
            Refresh();
        }

        public ICommand AddBandCommand
        {
            get { return new RelayCommand(AddBand, Band.IsValid); }
        }
        public void AddBand()
        {
            if (SelectedImagePad != null)
            {
                Band.Picture = convertStringToByte(SelectedImagePad);
            }
            else
            {
                Band.Picture = ImageSource;
            }
            Band.AddBand(Band);
            Refresh();
        }

        public ICommand DeleteBandCommand
        {
            get { return new RelayCommand<int>(DeleteBand); }
        }
        public void DeleteBand(int id)
        {
            if (Band.IsBandDeleteAllowed(id))
            {
                //Een bevestiging of de gebruiker wel degelijk deze contactpersoon wilt verwijderen
                var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Band.GetBandByID(id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
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
            else
            {
                MessageBox.Show("Deze band heeft nog optredens! U moet eerst alle optredens verwijderen voordat u deze band kunt verwijderen", "Opgelet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand DeleteGenreFromBandCommand
        {
            get { return new RelayCommand(DeleteGenreFromBand); }
        }

        public void DeleteGenreFromBand()
        {
            Band.DeleteGenre(SelectedBand, SelectedGenre);
            //Band.Genres.Remove(SelectedGenre);
            Bands = FestivalLib.model.Band.GetBands();
            GefilterdeBands = Bands;
            Genre = "";
            ImageSource = convertStringToByte("../../content/images/blank.jpg");
            Refresh();
        }

        public ICommand AddGenreToBandCommand
        {
            get { return new RelayCommand(AddGenreToBand); }
        }
        public void AddGenreToBand()
        {
            if (Genre != "")
            {
                //FestivalLib.model.Band.AddGenre(SelectedBand, Genre);
                Band.Genres.Add(FestivalLib.model.Genre.GetGenreByString(Genre));

                //Bands = FestivalLib.model.Band.GetBands();
                //GefilterdeBands = Bands;
                Genre = "";
                //ImageSource = convertStringToByte("../../content/images/blank.jpg");                
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("U kunt geen leeg genre meegeven", "Opgelet", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        #endregion

        #region Methods

        private static byte[] convertStringToByte(string sPad)
        {
            //volgende 4 lijnen code = om geselecteerde image om te zetten naar BLOB capable formaat
            byte[] btImage = null;
            FileStream fstStream = new FileStream(sPad, FileMode.Open, FileAccess.Read);
            BinaryReader brReader = new BinaryReader(fstStream);
            btImage = brReader.ReadBytes((int)fstStream.Length);
            return btImage;
        }

        public void Refresh()
        {
            Bands = Band.GetBands();
            GefilterdeBands = Bands;
            ImageSource = convertStringToByte("../../content/images/blank.jpg");
            Band = new Band();
            Band.Genres = new ObservableCollection<Genre>();
        }

        #endregion
    }
}
