using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        #region new lineup doorstuur waarden
        private Ypsilon2.model.Band _selectedDoorstuurBand;

        public Ypsilon2.model.Band SelectedDoorstuurBand
        {
            get { return _selectedDoorstuurBand; }
            set { _selectedDoorstuurBand = value; OnPropertyChanged("SelectedDoorstuurband"); }
        }

        private Stage _selectedDoorstuurStage;

        public Stage SelectedDoorstuurStage
        {
            get { return _selectedDoorstuurStage; }
            set { _selectedDoorstuurStage = value; OnPropertyChanged("SelectedDoorstuurStage"); }
        }

        private DateTime _selectedDoorstuurDate;

        public DateTime SelectedDoorstuurDate
        {
            get { return _selectedDoorstuurDate; }
            set { _selectedDoorstuurDate = value; OnPropertyChanged("SelectedDoorstuurDate"); }
        }
        
        
        
        #endregion

        #region commands
        public ICommand SaveLineUpCommand
        {
            get { return new RelayCommand(SaveLineUp); }
        }

        public void SaveLineUp()
        {
            Ypsilon2.model.LineUp lineup = new Ypsilon2.model.LineUp();

            lineup.Band = SelectedDoorstuurBand;
            lineup.Date = SelectedDoorstuurDate;
            lineup.Stage = SelectedDoorstuurStage.ID;

            lineup.From = SelectedDoorstuurDate;    //deze moet nog het geselecteerde uur worden
            lineup.Until = SelectedDoorstuurDate;   //deze moet nog het geselecteerde uur worden

            Ypsilon2.model.LineUp.AddLineUp(lineup);

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
        #endregion

    }
}
