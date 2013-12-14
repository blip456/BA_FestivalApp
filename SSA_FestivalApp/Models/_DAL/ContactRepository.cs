using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBHelper;
using System.Data.Common;
using System.Collections.ObjectModel;
using FestivalLib.model;

namespace SSA_FestivalApp.Models._DAL
{
    public class ContactRepository
    {
        public static ObservableCollection<Contactperson> GetContacts()
        {
            ObservableCollection<Contactperson> lstContacts = new ObservableCollection<Contactperson>();
            lstContacts = FestivalLib.model.Contactperson.GetContacts();
            return lstContacts;
        }
    }
}