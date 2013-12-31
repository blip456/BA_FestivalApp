using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ypsilon2.view;
using FestivalLib.model;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace Ypsilon2.viewmodel
{
    class ContactpersonVM : ObservableObject, IPage
    {
        #region fields en props

        public ContactpersonVM()
        {
            _contacts = FestivalLib.model.Contactperson.GetContacts();
            _gefilterdeContacts = Contacts;
            _contactTypes = ContactpersonType.GetContactTypes();
            _contact = new FestivalLib.model.Contactperson();            
        }

        public string Name
        {
            get { return "Contactpersonen"; }
        }

        private ObservableCollection<FestivalLib.model.Contactperson> _contacts;

        public ObservableCollection<FestivalLib.model.Contactperson> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; OnPropertyChanged("Contacts"); }
        }

        private ObservableCollection<FestivalLib.model.Contactperson> _gefilterdeContacts;

        public ObservableCollection<FestivalLib.model.Contactperson> GefilterdeContacts
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

        private ContactpersonType _selectedContactType;

        public ContactpersonType SelectedContactType
        {
            get { return _selectedContactType; }
            set { _selectedContactType = value; OnPropertyChanged("SelectedContactType"); }
        }


        private FestivalLib.model.Contactperson _selectedContact;

        public FestivalLib.model.Contactperson SelectedContact
        {
            get { return _selectedContact; }
            set 
            { 
                _selectedContact = value;
                //als er een contact geselecteerd is moet je dat contact persoon nemen en binden anders niet
                if (_selectedContact != null)
                {
                    Contact = SelectedContact;
                    //selected contact type moet nog aangepast worden zodat het type van de selected contact getoond wordt
                    SelectedContactType = SelectedContact.JobRole;
                }
                
                OnPropertyChanged("SelectedContact");               
            }
        }

        private FestivalLib.model.Contactperson _contact;

        public FestivalLib.model.Contactperson Contact
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
            get { return new RelayCommand(SaveContact, Contact.IsValid); }
        }
        public void SaveContact()
        {
            if (Contact.Name != "")
            {
                Contact.JobRole = SelectedContactType;
                //reeds bestaand contact aanpassen in DB
                FestivalLib.model.Contactperson.EditContact(Contact);
                //lijst met contacten opnieuw ophalen om als nieuw toegevoegd contact meteen te tonen in listview
                Contacts = FestivalLib.model.Contactperson.GetContacts();
                GefilterdeContacts = Contacts;

                //leegmaken van textboxen moet hier nog komen
                // ...
            }              
        }

        public ICommand AddContactCommand
        {
            get { return new RelayCommand(AddContact, Contact.IsValid); }
        }
        public void AddContact()
        {
            Contact.JobRole = SelectedContactType;
            //enkel als contactpersoon zijn naam is ingevuld mag hij/zij toegevoegd worden aan DB
            //normaal zou de gegevens validatie deze 'fout' moeten opvangen > deze if structuur is slechts een vangnet
            if (Contact.Name != "")
            {
                //contact toevoegen aan DB
                FestivalLib.model.Contactperson.AddContact(Contact);

                //lijst met contacten opnieuw ophalen om nieuw toegevoegd contact meteen te tonen in listview
                Contacts = FestivalLib.model.Contactperson.GetContacts();
                GefilterdeContacts = Contacts;

                //leegmaken van textboxen - werkt niet
                
            }   
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Gelieve op zijn minst een naam in te geven");
            }               
        }       

        public ICommand DeleteContactCommand
        {
            get { return new RelayCommand<int>(DeleteContact); }
        }
        public void DeleteContact(int id)
        {
            //Een bevestiging of de gebruiker wel degelijk deze contactpersoon wilt verwijderen
            // je zou ook Contact.Name kunnen gebruiken > deze kent enkel de Name als je eerst het contact selecteerd en dan verwijdered > GetContactByID kent .Name zonder een contact te selecteren
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + FestivalLib.model.Contactperson.GetContactByID(id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);            
            if (result == MessageBoxResult.OK)
            {
                FestivalLib.model.Contactperson.DeleteContact(id);
                Contacts = FestivalLib.model.Contactperson.GetContacts();
                GefilterdeContacts = Contacts;                
            }
            else
            {
                return;
            }            
        }
           
        public ICommand SearchCommand  
        {
            //Een gebruiker heeft de mogelijkheid om adhv een zoekbox te zoeken naar contactpersonen.
            //Bij ieder karakter dat ingedrukt wordt zal er gezocht op alle informatie worden en de gevonden contactpersonen terug geven
            get { return new RelayCommand<string>(Search); }
        }
        private void Search(string str)
        {                      
            Console.WriteLine(str);
            GefilterdeContacts = FestivalLib.model.Contactperson.GetContactsByString(Contacts, str);            
        }
        #endregion
    }
}
