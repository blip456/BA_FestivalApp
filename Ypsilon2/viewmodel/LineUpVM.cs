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
using Ypsilon2.view;

namespace Ypsilon2.viewmodel
{
    class LineUpVM : ObservableObject, IPage
    {
        private ObservableCollection<Stage> lstAlleStages = Stage.GetAlleStages();

        #region fields en props

        public LineUpVM()
        {
            _bands = Ypsilon2.model.Band.GetBands();
            _uniekeDagen = Ypsilon2.model.Festival.aantalDagen();
            _stagesPerDag = Stage.GetStagesByDay(SelectedDag);
            _lineUp = new Ypsilon2.model.LineUp();
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
                StagesPerDag = Stage.GetStagesByDay(SelectedDag);
            }
        }

        private ObservableCollection<DateTime> _uniekeDagen;

        public ObservableCollection<DateTime> UniekeDagen
        {
            get { return _uniekeDagen; }
            set { _uniekeDagen = value; OnPropertyChanged("UniekeDagen"); }
        }

        private ObservableCollection<Stage> _stagesPerDag;

        public ObservableCollection<Stage> StagesPerDag
        {
            get { return _stagesPerDag; }
            set { _stagesPerDag = value; OnPropertyChanged("StagesPerDag"); }
        }

        private ObservableCollection<Ypsilon2.model.Band> _bands;

        public ObservableCollection<Ypsilon2.model.Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        private Ypsilon2.model.LineUp _lineUp;

        public Ypsilon2.model.LineUp LineUp
        {
            get { return _lineUp; }
            set { _lineUp = value; OnPropertyChanged("LineUp"); }
        }

        #endregion

        #region commands
        public ICommand SaveLineUpCommand
        {
            get { return new RelayCommand(SaveLineUp); }
        }
        public void SaveLineUp()
        {
            Ypsilon2.model.LineUp.AddLineUp(LineUp);

            StagesPerDag = Stage.GetStagesByDay(SelectedDag);
        }

        public ICommand SaveStageCommand
        {
            get { return new RelayCommand<string>(SaveStage); }
        }
        public void SaveStage(string NewPodiumName)
        {
            Stage.AddStage(NewPodiumName);
            StagesPerDag = Stage.GetStagesByDay(SelectedDag);
        }

        public ICommand DeleteStageCommand
        {
            get { return new RelayCommand<int>(DeleteStage); }
        }
        public void DeleteStage(int id)
        {
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Stage.GetStageByID(id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                Stage.DeleteStage(id);
                StagesPerDag = Stage.GetStagesByDay(SelectedDag);
            }
            else
            {
                return;
            }
        }

        public ICommand DeleteBandFromLineUpCommand
        {
            get { return new RelayCommand<int>(DeleteBandFromLineUp); }
        }
        public void DeleteBandFromLineUp(int id)
        {
            int bandID = Convert.ToInt32(model.LineUp.GetLineUpByID(id).Band.ID);
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Ypsilon2.model.Band.GetBandByID(Bands, bandID).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {                
                Ypsilon2.model.Band.DeleteBandFromLineUp(id);
                Bands = Ypsilon2.model.Band.GetBands();
                StagesPerDag = Stage.GetStagesByDay(SelectedDag);
            }
            else
            {
                return;
            }
        }
        #endregion

    }
}
