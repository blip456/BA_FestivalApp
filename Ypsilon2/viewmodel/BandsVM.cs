﻿using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ypsilon2.model;
using System.IO;

namespace Ypsilon2.viewmodel
{
    class BandsVM : ObservableObject, IPage
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
            _imageSource = convertStringToByte("../../content/images/blank.jpg");
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
                _selectedBand = value; OnPropertyChanged("SelectedBand");
                if (SelectedBand != null)
                {
                    if (SelectedBand.Picture != null && SelectedBand.Picture.Length > 6)
                    {
                        ImageSource = SelectedBand.Picture;
                    }
                    else
                    {
                        ImageSource = convertStringToByte("../../content/images/blank.jpg");
                    }
                }

            }
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

        #endregion

        private static byte[] convertStringToByte(string sPad)
        {
            //volgende 4 lijnen code = om geselecteerde image om te zetten naar BLOB capable formaat
            byte[] btImage = null;
            FileStream fstStream = new FileStream(sPad, FileMode.Open, FileAccess.Read);
            BinaryReader brReader = new BinaryReader(fstStream);
            btImage = brReader.ReadBytes((int)fstStream.Length);
            return btImage;
        }

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

        public ICommand SelectImageCommand
        {
            get { return new RelayCommand(SelectImage); }
        }

        private void SelectImage()
        {
            Console.WriteLine("FILE DIALOG");
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
            get { return new RelayCommand<Band>(SaveBand); }
        }

        public void SaveBand(Band band)
        {
            band.Picture = ImageSource;
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
            if (SelectedImagePad != null)
            {
                band.Picture = convertStringToByte(SelectedImagePad);
            }
            else
            {
                band.Picture = ImageSource;
            }            
            band.Name = strings;
            Band.AddBand(band);
            GefilterdeBands = Band.GetBands();

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
