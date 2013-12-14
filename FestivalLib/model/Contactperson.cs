using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;
using System.ComponentModel;

namespace FestivalLib.model
{
    public class Contactperson:IDataErrorInfo
    {
        #region field en properties
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _company;
        
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private ContactpersonType _jobRole;

        public ContactpersonType JobRole
        {
            get { return _jobRole; }
            set { _jobRole = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _cellphone;

        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }

        #endregion

        #region Validation
        public string Error
        {
            get {return "Er is een fout!"; }
        }

        public string this[string columnName]
        {
            get 
            {
                switch (columnName)
                {
                    case "Name":
                        if (Name != null && Name.Length <=2)
                        {
                            return "De naam moet minstens 3 karakters hebben";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        #endregion

        #region SQL
        private static Contactperson CreateContact(DbDataReader reader)
        {
            Contactperson contact = new Contactperson();
            contact.ID = Convert.ToString(reader["contactperson_id"]);
            contact.Name = Convert.ToString(reader["contactperson_name"]);
            contact.Company = Convert.ToString(reader["contactperson_company"]);
            contact.JobRole = ContactpersonType.GetContactTypeByID((int)reader["contactpersontype_idJobrole"]);
            contact.City = Convert.ToString(reader["contactperson_city"]);
            contact.Email = Convert.ToString(reader["contactperson_email"]);
            contact.Phone = Convert.ToString(reader["contactperson_phone"]);
            contact.Cellphone = Convert.ToString(reader["contactperson_cell"]);
            return contact;
        }

        public static ObservableCollection<Contactperson> GetContacts()
        {
            ObservableCollection<Contactperson> lstContacts = new ObservableCollection<Contactperson>();
            DbDataReader reader = Database.GetData("SELECT * FROM contactperson");
            while (reader.Read())
            {
                lstContacts.Add(CreateContact(reader));
            }
            return lstContacts;
        }

        public static Contactperson GetContactByID(ObservableCollection<Contactperson> lst, int id)
        {
            Contactperson gevondenContact = new Contactperson();
            foreach (Contactperson contact in lst)
            {
                if (contact.ID == Convert.ToString(id))
                {
                    gevondenContact = contact;
                }
            }
            return gevondenContact;
        }

        public static ObservableCollection<Contactperson> GetContactsByString(ObservableCollection<Contactperson> lst, string search)
        {
            ObservableCollection<Contactperson> lstGevondenContacts = new ObservableCollection<Contactperson>();
            foreach (Contactperson contact in lst)
            {
                if (contact.Name.ToUpper().Contains(search.ToUpper()) || contact.City.ToUpper().Contains(search.ToUpper()) || contact.Company.ToUpper().Contains(search.ToUpper()) || contact.Email.ToUpper().Contains(search.ToUpper()) || contact.Cellphone.ToUpper().Contains(search.ToUpper()) || contact.Phone.ToUpper().Contains(search.ToUpper()))
                {
                    lstGevondenContacts.Add(contact);
                }
            }

            return lstGevondenContacts;
        }

        public static void AddContact(Contactperson contact)
        {
            string sql = "INSERT INTO contactperson(contactperson_name, contactperson_company, contactperson_city, contactperson_email, contactperson_phone, contactperson_cell, contactpersontype_idJobrole) VALUES (@name, @company, @city, @email, @phone, @cell, @idjobrole);";

            DbParameter par1 = Database.AddParameter("@name", contact.Name);
            DbParameter par2 = Database.AddParameter("@company", contact.Company);
            DbParameter par3 = Database.AddParameter("@city", contact.City);
            DbParameter par4 = Database.AddParameter("@email", contact.Email);
            DbParameter par5 = Database.AddParameter("@phone", contact.Phone);
            DbParameter par6 = Database.AddParameter("@cell", contact.Cellphone);
            DbParameter par7 = Database.AddParameter("@idjobrole", contact.JobRole.ID);

            int i = Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7);
            if (i == 0)
            {
                MessageBox.Show("Toevoegen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
            }
            Console.WriteLine(i + " row(s) are affected");
        }

        public static void EditContact(Contactperson contact)
        {
            string sql = "UPDATE contactperson SET contactperson_name=@name, contactperson_company=@company, contactperson_city=@city, contactperson_email=@email, contactperson_phone=@phone, contactperson_cell=@cell, contactpersontype_idJobrole=@idjobrole WHERE contactperson_id=@ID;";

            DbParameter par1 = Database.AddParameter("@name", contact.Name);
            DbParameter par2 = Database.AddParameter("@company", contact.Company);
            DbParameter par3 = Database.AddParameter("@city", contact.City);
            DbParameter par4 = Database.AddParameter("@email", contact.Email);
            DbParameter par5 = Database.AddParameter("@phone", contact.Phone);
            DbParameter par6 = Database.AddParameter("@cell", contact.Cellphone);
            DbParameter par7 = Database.AddParameter("@idjobrole", contact.JobRole.ID);

            DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(contact.ID));

            int i = Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7, parID);
            if (i == 0)
            {
                MessageBox.Show("Opslaan mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
            }
            Console.WriteLine(i + " row(s) are affected");
        }

        public static void DeleteContact(int id)
        {
            string sql = "DELETE FROM contactperson WHERE contactperson_id = @ID;";

            DbParameter parID = Database.AddParameter("@ID", id);

            int i = Database.ModifyData(sql, parID);
            if (i == 0)
            {
                MessageBox.Show("Verwijderen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
            }
            Console.WriteLine(i + " row(s) are deleted");
        }
        #endregion
    }
}
