using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FestivalLib.model;
using Ypsilon2.viewmodel;

namespace Ypsilon2.viewmodel
{
    class TicketingVM : ObservableObject, IPage
    {
        #region Fields en props
        public TicketingVM()
        {
            _reserveringen = Ticket.GetTickets();
            _ticketTypes = TicketType.GetTicketTypes();

            _ticketTypesNormaal = TicketType.GetTicketTypeByString("Normaal");
            _ticketTypesVip = TicketType.GetTicketTypeByString("VIP");

            _soldTicketsNormal = Ticket.SoldTickets(Reserveringen)[1];
            _soldTicketsVip = Ticket.SoldTickets(Reserveringen)[0];

            _totalTicketTypeNormal = TicketType.CountTotalNormal(TicketTypes);
            _totalTicketTypeVip = TicketType.CountTotalVip(TicketTypes);            
        }

        public string Name
        {
            get { return "Ticketing"; }
        }

        private ObservableCollection<Ticket> _reserveringen;

        public ObservableCollection<Ticket> Reserveringen
        {
            get { return _reserveringen; }
            set { _reserveringen = value; OnPropertyChanged("Reserveringen"); }
        }

        private ObservableCollection<TicketType> _ticketTypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _ticketTypes; }
            set { _ticketTypes = value; OnPropertyChanged("TicketTypes"); }
        }

        private ObservableCollection<TicketType> _ticketTypesNormaal;

        public ObservableCollection<TicketType> TicketTypesNormaal
        {
            get { return _ticketTypesNormaal; }
            set { _ticketTypesNormaal = value; OnPropertyChanged("TicketTypesNormaal"); }
        }

        private ObservableCollection<TicketType> _ticketTypesVip;

        public ObservableCollection<TicketType> TicketTypesVip
        {
            get { return _ticketTypesVip; }
            set { _ticketTypesVip = value; OnPropertyChanged("TicketTypesVIp"); }
        }

        private TicketType _selectedDagNormaal;

        public TicketType SelectedDagNormaal
        {
            get { return _selectedDagNormaal; }
            set { _selectedDagNormaal = value; OnPropertyChanged("SelectedDagNormaal"); }
        }

        private TicketType _selectedDagVip;

        public TicketType SelectedDagVip
        {
            get { return _selectedDagVip; }
            set { _selectedDagVip = value; }
        }
        #endregion
        
        //Deze values zijn gebined aan de textboxen voor een reserveringen te plaatsen.
        //Via properties zal er een ticket object gemaakt worden en doorgestuurd naar de AddTicket methode
        #region Doorstuur properties

        private TicketType _selectedDagReservering;

        public TicketType SelectedDagReservering
        {
            get { return _selectedDagReservering; }
            set { _selectedDagReservering = value; OnPropertyChanged("SelectedDagReservering"); }
        }

        private string _voornaam;

        public string Voornaam
        {
            get { return _voornaam; }
            set { _voornaam = value; OnPropertyChanged("Voornaam"); }
        }

        private string _achternaam;

        public string Achternaam
        {
            get { return _achternaam; }
            set { _achternaam = value; OnPropertyChanged("Achternaam"); }
        }

        private string _emailadres;

        public string Emailadres
        {
            get { return _emailadres; }
            set { _emailadres = value; OnPropertyChanged("Emailadres"); }
        }

        private int _aantalTickets;

        public int AantalTickets
        {
            get { return _aantalTickets; }
            set { _aantalTickets = value; OnPropertyChanged("AantalTickets"); }
        }
        #endregion

        //Hier zitten de totaal aantal verkochte tickets in voor zowel normaal als VIP.
        //Alsook zit hier het totaal aantal beschikbare tickets in
        #region TicketValues

        private int _soldTicketsNormal;

        public int SoldTicketNormal
        {
            get { return _soldTicketsNormal; }
            set { _soldTicketsNormal = value; OnPropertyChanged("SoldTicketsNormal"); }
        }

        private int _soldTicketsVip;

        public int SoldTicketVip
        {
            get { return _soldTicketsVip; }
            set { _soldTicketsVip = value; OnPropertyChanged("SoldTicketsVip"); }
        }

        private int _totalTicketTypeNormal;

        public int TotalTicketTypeNormal
        {
            get { return _totalTicketTypeNormal; }
            set { _totalTicketTypeNormal = value; OnPropertyChanged("TotalTicketTypeNormal"); }
        }

        private int _totalTicketTypeVip;

        public int TotalTicketTypeVip
        {
            get { return _totalTicketTypeVip; }
            set { _totalTicketTypeVip = value; OnPropertyChanged("TotalTicketTypeVip"); }
        }
        #endregion

        #region Commands
        public ICommand SaveTicketTypeNormaalCommand
        {
            get { return new RelayCommand<TicketType>(SaveTicketTypeNormaal); }
        }

        private void SaveTicketTypeNormaal(TicketType ticketType)
        {
            TicketType.EditTicketType(ticketType);
            TotalTicketTypeNormal = TicketType.CountTotalNormal(TicketTypes);
        }

        public ICommand SaveTicketTypeVipCommand
        {
            get { return new RelayCommand<TicketType>(SaveTicketTypeVip); }
        }

        private void SaveTicketTypeVip(TicketType ticketType)
        {
            TicketType.EditTicketType(ticketType);
            TotalTicketTypeVip = TicketType.CountTotalVip(TicketTypes);
        }

        public ICommand DeleteReserveringCommand
        {
            get { return new RelayCommand<int>(DeleteReservering); }
        }

        private void DeleteReservering(int id)
        {
            Ticket.DeleteTicket(id);
            Reserveringen = Ticket.GetTickets();
        }

        public ICommand ReserveerCommand
        {
            get { return new RelayCommand(Reserveer); }
        }

        public void Reserveer()
        {
            Ticket ticket = new Ticket();
            ticket.TicketHolder = _voornaam + " " + _achternaam;
            ticket.TicketHolderEmail = _emailadres;
            ticket.Amount = _aantalTickets;
            ticket.TicketType = _selectedDagReservering;

            Ticket.AddTicket(ticket);

            Emailadres = "";
            Voornaam = "";
            Achternaam = "";


            Reserveringen = Ticket.GetTickets();
            SoldTicketNormal = Ticket.SoldTickets(Reserveringen)[1];
            SoldTicketVip = Ticket.SoldTickets(Reserveringen)[0];
        }
        #endregion
       

    }
}
