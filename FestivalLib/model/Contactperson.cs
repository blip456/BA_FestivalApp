using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using Xceed.Wpf.Toolkit;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FestivalLib.model
{
    public class Contactperson : IDataErrorInfo
    {
        #region field en properties
        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        [Required(ErrorMessage = "U moet een naam invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een naam moet tussen de 3 en 50 karakters liggen")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _company;
        [Required(ErrorMessage = "U moet een bedrijfsnaam invullen")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Een bedrijfsnaam moet tussen de 3 en 50 karakters liggen")]
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
        [Required(ErrorMessage = "U moet een emailadres invullen")]
        [EmailAddress(ErrorMessage = "Een emailadres moet van het formaat 'example@example.org' zijn")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;
        [Required(ErrorMessage = "U moet een telefoon nummer invullen")]
        [RegularExpression(@"^([0-9]{3}/|[0-9]{2}/)[0-9]{2}.[0-9]{2}.[0-9]{2}$")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _cellphone;
        [Required(ErrorMessage = "U moet een gsm nummer invullen")]
        [RegularExpression(@"^[0-9]{4}/[0-9]{2}.[0-9]{2}.[0-9]{2}$")]
        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }

        #endregion

        #region Errorhandling
        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columName
                    });
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return string.Empty;
            }

        }
        #endregion

        #region Enable/Disable Controls
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null),
            null, true);
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
            if (reader != null)
                reader.Close();
            return lstContacts;
        }

        public static Contactperson GetContactByID(int id)
        {
            Contactperson gevondenContact = new Contactperson();
            string sql = "SELECT * FROM contactperson WHERE contactperson_id=@id;";
            DbParameter parID = Database.AddParameter("@ID", id);
            DbDataReader reader = Database.GetData(sql, parID);
            while (reader.Read())
            {
                gevondenContact = CreateContact(reader);
            }
            if (reader != null)
                reader.Close();
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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static void EditContact(Contactperson contact)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static void DeleteContact(int id)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        #endregion
    }
}
