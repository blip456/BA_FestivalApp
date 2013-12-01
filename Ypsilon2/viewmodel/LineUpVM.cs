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

        #region fields en props
        public LineUpVM()
        {
            //_lineUps = Ypsilon2.model.LineUp.GetLineUp();          
            //_lineUps = model.LineUp.GetLineUpsVersie2();
            _bands = Ypsilon2.model.Band.GetBands();
            _festivals = Ypsilon2.model.Festival.GetFestivals();
            _stages = Stage.GetStages();
        }


        public string Name
        {
            get { return "Line Up"; }
        }

        private Ypsilon2.model.Festival _festivals;

        public Ypsilon2.model.Festival Festivals
        {
            get { return _festivals; }
            set { _festivals = value; OnPropertyChanged("Festivals"); }
        }

        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }
        
        

        //private ObservableCollection<Ypsilon2.model.LineUp> _lineUps;

        //public ObservableCollection<Ypsilon2.model.LineUp> LineUps
        //{
        //    get { return _lineUps; }
        //    set { _lineUps = value; }
        //}

        private Dictionary<string, ObservableCollection<model.LineUp>> _lineUps;

        public Dictionary<string, ObservableCollection<model.LineUp>> LineUps
        {
            get { return _lineUps; }
            set { _lineUps = value; }
        }

        private ObservableCollection<Ypsilon2.model.Band> _bands;

        public ObservableCollection<Ypsilon2.model.Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        private ObservableCollection<Ypsilon2.model.LineUp> _uniquePodia;

        public ObservableCollection<Ypsilon2.model.LineUp> UniquePodia
        {
            get { return _uniquePodia; }
            set { _uniquePodia = value; OnPropertyChanged("UniquePodia"); }
        }
        
        #endregion
        

        #region commands

        public ICommand SaveFestivalCommand
        {
            get { return new RelayCommand(SaveFestival); }
        }

        public void SaveFestival()
        {
            Ypsilon2.model.Festival.EditFestival(Festivals);
        }

        public ICommand SaveLineUpCommand
        {
            get { return new RelayCommand(SaveLineUp); }
        }

        public void SaveLineUp()
        {
           
        }

        public ICommand SaveStageCommand
        {
            get { return new RelayCommand<string>(SaveStage); }
        }

        public void SaveStage(string NewPodiumName)
        {
            Stage.AddStage(NewPodiumName);
            Stages = Stage.GetStages();
        }
        #endregion

    }
}
