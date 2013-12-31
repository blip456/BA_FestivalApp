using FestivalLib.model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Ypsilon2.viewmodel
{
    class LineUpVM : ObservableObject, IPage
    {
        //private ObservableCollection<FestivalLib.model.Stage> lstAlleStages = FestivalLib.model.Stage.GetAlleStages();

        #region fields en props

        public LineUpVM()
        {
            _bands = FestivalLib.model.Band.GetBands();
            _uniekeDagen = FestivalLib.model.Festival.aantalDagen();
            _stagesPerDag = FestivalLib.model.Stage.GetStagesByDay(DateTime.Now);
            _lineUp = new FestivalLib.model.LineUp();
        }

        public string Name
        {
            get { return "Line Up"; }
        }

        private DateTime _selectedDag;

        public DateTime SelectedDag
        {
            get { return _selectedDag; }
            set
            {
                _selectedDag = value;
                OnPropertyChanged("SelectedDag");
                StagesPerDag = FestivalLib.model.Stage.GetStagesByDay(SelectedDag);
            }
        }

        private ObservableCollection<DateTime> _uniekeDagen;

        public ObservableCollection<DateTime> UniekeDagen
        {
            get { return _uniekeDagen; }
            set { _uniekeDagen = value; OnPropertyChanged("UniekeDagen"); }
        }

        private ObservableCollection<FestivalLib.model.Stage> _stagesPerDag;

        public ObservableCollection<FestivalLib.model.Stage> StagesPerDag
        {
            get { return _stagesPerDag; }
            set { _stagesPerDag = value; OnPropertyChanged("StagesPerDag"); }
        }

        private ObservableCollection<FestivalLib.model.Band> _bands;

        public ObservableCollection<FestivalLib.model.Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        private FestivalLib.model.LineUp _lineUp;

        public FestivalLib.model.LineUp LineUp
        {
            get { return _lineUp; }
            set { _lineUp = value; OnPropertyChanged("LineUp"); }
        }

        private Stage _selectedStageEdit;

        public Stage SelectedStageEdit
        {
            get { return _selectedStageEdit; }
            set { _selectedStageEdit = value; OnPropertyChanged("SelectedStageEdit"); }
        }
        

        #endregion

        #region commands
        public ICommand SaveLineUpCommand
        {
            get { return new RelayCommand(SaveLineUp, LineUp.IsValid); }
        }
        public void SaveLineUp()
        {
            FestivalLib.model.LineUp.AddLineUp(LineUp);

            StagesPerDag = FestivalLib.model.Stage.GetStagesByDay(SelectedDag);
        }

        public ICommand SaveStageCommand
        {
            get { return new RelayCommand<string>(SaveStage); }
        }

        public void SaveStage(string NewPodiumName)
        {
            Stage.EditStage(NewPodiumName, SelectedStageEdit);
            StagesPerDag = Stage.GetStagesByDay(SelectedDag);
        }

        public ICommand AddStageCommand
        {
            get { return new RelayCommand<string>(AddStage); }
        }
        public void AddStage(string NewPodiumName)
        {
            FestivalLib.model.Stage.AddStage(NewPodiumName);
            StagesPerDag = FestivalLib.model.Stage.GetStagesByDay(SelectedDag);
        }

        public ICommand DeleteStageCommand
        {
            get { return new RelayCommand<int>(DeleteStage); }
        }
        public void DeleteStage(int id)
        {
            if (Stage.IsStageDeleteAllowed(id))
            {
                var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + FestivalLib.model.Stage.GetStageByID(id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    FestivalLib.model.Stage.DeleteStage(id);
                    StagesPerDag = FestivalLib.model.Stage.GetStagesByDay(SelectedDag);
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Deze stage heeft nog optredens! U moet eerst alle optredens verwijderen voordat u deze stage kunt verwijderen", "Opgelet", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public ICommand DeleteBandFromLineUpCommand
        {
            get { return new RelayCommand<int>(DeleteBandFromLineUp); }
        }
        public void DeleteBandFromLineUp(int id)
        {
            int bandID = Convert.ToInt32(FestivalLib.model.LineUp.GetLineUpByID(id).Band.ID);
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + FestivalLib.model.Band.GetBandByID(bandID).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                FestivalLib.model.Band.DeleteBandFromLineUp(id);
                Bands = FestivalLib.model.Band.GetBands();
                StagesPerDag = FestivalLib.model.Stage.GetStagesByDay(SelectedDag);
            }
            else
            {
                return;
            }
        }
        #endregion

    }
}
