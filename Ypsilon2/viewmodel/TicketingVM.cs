﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FestivalLib.model;
using Ypsilon2.viewmodel;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Windows;

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

            _reservering = new Ticket();
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

        private Ticket _reservering;

        public Ticket Reservering
        {
            get { return _reservering; }
            set { _reservering = value; OnPropertyChanged("Reservering"); }
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
        // ticket type normaal opslaan - je kan enkel de prijs en hoeveelheid aanpassen
        public ICommand SaveTicketTypeNormaalCommand
        {
            get { return new RelayCommand<TicketType>(SaveTicketTypeNormaal); }
        }
        private void SaveTicketTypeNormaal(TicketType ticketType)
        {
            TicketType.EditTicketType(ticketType);
            TotalTicketTypeNormal = TicketType.CountTotalNormal(TicketTypes);

            Refresh();
        }

        //tickettype vip opslaan - je kan enkel de prijs en hoeveelheid aanpassen
        public ICommand SaveTicketTypeVipCommand
        {
            get { return new RelayCommand<TicketType>(SaveTicketTypeVip); }
        }
        private void SaveTicketTypeVip(TicketType ticketType)
        {
            TicketType.EditTicketType(ticketType);
            TotalTicketTypeVip = TicketType.CountTotalVip(TicketTypes);

            Refresh();
        }

        //een gereserveerd ticket verwijderen
        public ICommand DeleteReserveringCommand
        {
            get { return new RelayCommand<int>(DeleteReservering); }
        }
        private void DeleteReservering(int id)
        {
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Ticket.GetTicketByID(id).TicketHolder + " zijn reservering te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                Ticket.DeleteTicket(id);
                Refresh();
            }
            else
            {
                return;
            }

            Ticket.DeleteTicket(id);
            Reserveringen = Ticket.GetTickets();

            Refresh();
        }

        //een nieuw tiket reserveren
        public ICommand ReserveerCommand
        {
            get { return new RelayCommand(Reserveer, Reservering.IsValid); }
        }
        public void Reserveer()
        {            
            Ticket.AddTicket(Reservering);

            Reserveringen = Ticket.GetTickets();
            SoldTicketNormal = Ticket.SoldTickets(Reserveringen)[1];
            SoldTicketVip = Ticket.SoldTickets(Reserveringen)[0];
            
            Refresh();
        }

        //een ticket van een bepaalde user afdrukken naar Word
        public ICommand PrintTicketCommand
        {
            get { return new RelayCommand<int>(PrintTicket); }
        }
        private void PrintTicket(int id)
        {
            Ticket ticket = Ticket.GetTicketByID(id);
            Festival festival = Festival.GetFestivals();            
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sPad = fbd.SelectedPath;
                Ticket.PrintWord(ticket, festival, sPad);
            }            
        }
        #endregion

        #region Methods
        //VM refreshen
        public void Refresh()
        {
            Reservering = new Ticket();

            Reserveringen = Ticket.GetTickets();
            TicketTypes = TicketType.GetTicketTypes();

            TicketTypesNormaal = TicketType.GetTicketTypeByString("Normaal");
            TicketTypesVip = TicketType.GetTicketTypeByString("VIP");

            TotalTicketTypeNormal = TicketType.CountTotalNormal(TicketTypes);
            TotalTicketTypeVip = TicketType.CountTotalVip(TicketTypes);

            SoldTicketNormal = Ticket.SoldTickets(Reserveringen)[1];
            SoldTicketVip = Ticket.SoldTickets(Reserveringen)[0];
        }
        #endregion

    }
}
