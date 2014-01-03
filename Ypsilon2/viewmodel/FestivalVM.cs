using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FestivalLib.model;
//using FestivalLib.Model;

namespace Ypsilon2.viewmodel
{
    class FestivalVM : ObservableObject, IPage
    {
        #region fields en props
        public string Name
        {
            get { return "Festivalgegevens"; }
        }

        public FestivalVM()
        {
            _festivals = Festival.GetFestivals();
        }
        private Festival _festivals;

        public Festival Festivals
        {
            get { return _festivals; }
            set { _festivals = value; OnPropertyChanged("Festivals"); }
        }
        #endregion

        public ICommand SaveFestivalCommand
        {
            get { return new RelayCommand(SaveFestival, Festivals.IsValid); }
        }

        public void SaveFestival()
        {           
            Festival.EditFestival(Festivals);
            Festival.ChangeTicketDates();
            Festivals = Festival.GetFestivals();
            Xceed.Wpf.Toolkit.MessageBox.Show("Controleer de tickets en overschrijf eventueel de standaard waarden", "Opgelet", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
    }
}
