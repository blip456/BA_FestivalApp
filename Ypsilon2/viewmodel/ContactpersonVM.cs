using GalaSoft.MvvmLight.Command;
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
                //als er een contact geselecteerd is moet je dat contact persoon nemen en binden anders niet
                if (_selectedContact != null)
                {
                    Contact = SelectedContact;
                }
                
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
            if (Contact.Name != "")
            {
                  //reeds bestaand contact aanpassen in DB
                Ypsilon2.model.Contactperson.EditContact(Contact);
                //lijst met contacten opnieuw ophalen om als nieuw toegevoegd contact meteen te tonen in listview
                Contacts = Ypsilon2.model.Contactperson.GetContacts();
                GefilterdeContacts = Contacts;

                //leegmaken van textboxen moet hier nog komen
                // ...
            }              
        }

        public ICommand AddContactCommand
        {
            get { return new RelayCommand(AddContact); }
        }
        public void AddContact()
        {
            //enkel als contactpersoon zijn naam is ingevuld mag hij/zij toegevoegd worden aan DB
            //normaal zou de gegevens validatie deze 'fout' moeten opvangen > deze if structuur is slechts een vangnet
            if (Contact.Name != "")
            {
                //contact toevoegen aan DB
                Ypsilon2.model.Contactperson.AddContact(Contact);

                //lijst met contacten opnieuw ophalen om nieuw toegevoegd contact meteen te tonen in listview
                Contacts = Ypsilon2.model.Contactperson.GetContacts();
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
            var result = Xceed.Wpf.Toolkit.MessageBox.Show("U staat op het punt om " + Ypsilon2.model.Contactperson.GetContactByID(Contacts, id).Name + " te verwijderen", "Opgelet", MessageBoxButton.OKCancel, MessageBoxImage.Warning);            
            if (result == MessageBoxResult.OK)
            {
                Ypsilon2.model.Contactperson.DeleteContact(id);
                Contacts = Ypsilon2.model.Contactperson.GetContacts();
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
            GefilterdeContacts = model.Contactperson.GetContactsByString(Contacts, str);            
        }
        #endregion
    }
}
