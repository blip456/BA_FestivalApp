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
            get { return new RelayCommand(SaveFestival); }
        }

        public void SaveFestival()
        {
            Festival festival = new Festival();
            festival.ID = Festivals.ID;
            festival.Name = Festivals.Name;
            festival.Omschrijving = Festivals.Omschrijving;
            festival.StartDate = Festivals.StartDate;
            festival.EndDate = Festivals.EndDate;
            Festival.EditFestival(festival);
            Festivals = Festival.GetFestivals();
        }
    }
}
