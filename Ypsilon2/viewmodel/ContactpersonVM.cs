﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ypsilon2.view;
using Ypsilon2.model;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace Ypsilon2.viewmodel
{
    class ContactpersonVM : ObservableObject, IPage
    {
        #region fields en props

        public ContactpersonVM()
        {
            _contacts = Ypsilon2.model.Contactperson.GetContacts();
            _gefilterdeContacts = Contacts;
            _contactTypes = ContactpersonType.GetContactTypes();
            _contact = new Ypsilon2.model.Contactperson();
        }

        public string Name
        {
            get { return "Contactpersonen"; }
        }

        private ObservableCollection<Ypsilon2.model.Contactperson> _contacts;

        public ObservableCollection<Ypsilon2.model.Contactperson> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; OnPropertyChanged("Contacts"); }
        }

        private ObservableCollection<Ypsilon2.model.Contactperson> _gefilterdeContacts;

        public ObservableCollection<Ypsilon2.model.Contactperson> GefilterdeContacts
        {
            get { return _gefilterdeContacts; }
            set { _gefilterdeContacts = value; OnPropertyChanged("GefilterdeContacts"); }
        }
        

        private ObservableCollection<ContactpersonType> _contactTypes;

        public ObservableCollection<ContactpersonType> ContactTypes
        {
            get { return _contactTypes; }
            set { _contactTypes = value; OnPropertyChanged("ContactTypes"); }
        }

        private Ypsilon2.model.Contactperson _selectedContact;

        public Ypsilon2.model.Contactperson SelectedContact
        {
            get { return _selectedContact; }
            set 
            { 
                _selectedContact = value;
                Contact = SelectedContact;
                OnPropertyChanged("SelectedContact");
                
            }
        }

        private Ypsilon2.model.Contactperson _contact;

        public Ypsilon2.model.Contactperson Contact
        {
            get { return _contact; }
            set { _contact = value; OnPropertyChanged("Contact"); }
        }
        

        private string _searchtext;
        public string SearchText
        {
            get { return _searchtext; }
            set { _searchtext = value; OnPropertyChanged("SearchText"); }
        }
      
        #endregion

        #region Commands

        public ICommand SaveContactCommand
        {
            get { return new RelayCommand(SaveContact); }
        }

        public void SaveContact()
        {
            Ypsilon2.model.Contactperson.EditContact(Contact);
            GefilterdeContacts = Ypsilon2.model.Contactperson.GetContacts();
        }

        public ICommand AddContactCommand
        {
            get { return new RelayCommand(AddContact); }
        }
        public void AddContact()
        {           
            Ypsilon2.model.Contactperson contact = new Ypsilon2.model.Contactperson();           
            contact.Company = Contact.Company;
            contact.Email = Contact.Email;
            contact.City = Contact.City;
            contact.Phone = Contact.Phone;
            contact.Cellphone = Contact.Cellphone;
            contact.Name = Contact.Name;
            //JOBROLE NOG AAN CONTACT TOEVOEGEN

            Ypsilon2.model.Contactperson.AddContact(contact);
            GefilterdeContacts = Ypsilon2.model.Contactperson.GetContacts();
            
        }       

        public ICommand DeleteContactCommand
        {
            get { return new RelayCommand<int>(DeleteContact); }
        }

        public void DeleteContact(int id)
        {
            //Een bevestiging of de gebruiker wel degelijk deze contactpersoon wilt verwijderen
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Ypsilon2.model.Contactperson.GetContactByID(Contacts,id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);            
            if (result == MessageBoxResult.OK)
            {
                Ypsilon2.model.Contactperson.DeleteContact(id);
                GefilterdeContacts = Ypsilon2.model.Contactperson.GetContacts();
            }
            else
            {
                return;
            }            
        }
        
        //Een gebruiker heeft de mogelijkheid om adhv een zoekvenster te zoeken naar contactpersonen.
        //Bij ieder karakter dat ingedrukt wordt zal er gezocht worden en de gevonden contactpersonen terug geven
        public ICommand SearchCommand  
        {
            get { return new RelayCommand<string>(Search); }
        }

        private void Search(string str)
        {                      
            Console.WriteLine(str);
            GefilterdeContacts = model.Contactperson.GetContactsByString(Contacts, str);            
        }
        #endregion

    }
}
