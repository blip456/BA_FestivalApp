using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class ContactpersonType
    {
        #region Field en properties
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

        #endregion

        #region SQL
        // een concact type object maken
        private static ContactpersonType CreateContactType(DbDataReader reader)
        {
            try
            {
                ContactpersonType contactType = new ContactpersonType();
                contactType.ID = Convert.ToString(reader["contactpersontype_id"]);
                contactType.Name = Convert.ToString(reader["contactpersontype_name"]);
                return contactType;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create contacttype: " + ex.Message);
                return null;
            }
        }

        //alle contacttpes ophalen
        public static ObservableCollection<ContactpersonType> GetContactTypes()
        {
            try
            {
                ObservableCollection<ContactpersonType> lstContactTypes = new ObservableCollection<ContactpersonType>();
                DbDataReader reader = Database.GetData("SELECT * FROM contactpersontype");
                while (reader.Read())
                {
                    lstContactTypes.Add(CreateContactType(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstContactTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get contact types: " + ex.Message);
                return null;
            }
        }

        //1 contacttype ophalen adhv en ID
        public static ContactpersonType GetContactTypeByID(int id)
        {
            try
            {
                ContactpersonType gevondenContactType = new ContactpersonType();
                gevondenContactType = lstContactTypes.Single(i => i.ID == Convert.ToString(id));
                return gevondenContactType;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get contactype by id: " + ex.Message);
                return null;
            }
        }

        //een contacttype toevoegen aan de DB - wordt niet gebruikt maar is beschikbaar
        public static void AddContactType(ContactpersonType contactType)
        {
            try
            {
                string sql = "INSERT INTO contactpersontype(contactpersontype_name) VALUES (@name);";

                DbParameter par1 = Database.AddParameter("@name", contactType.Name);

                int i = Database.ModifyData(sql, par1);
                if (i == 0)
                {
                    MessageBox.Show("Toevoegen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected in contactpersontype");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add contacttype: " + ex.Message);
            }
        }

        //een contacttype aanpassen in de DB - wordt niet gebruikt maar is beschikbaar
        public static void EditContactType(ContactpersonType contactType)
        {
            try
            {
                string sql = "UPDATE contactpersontype SET contactpersontype_name=@name WHERE contactpersontype_id=@ID;";

                DbParameter par1 = Database.AddParameter("@name", contactType.Name);

                DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(contactType.ID));

                int i = Database.ModifyData(sql, par1, parID);
                if (i == 0)
                {
                    MessageBox.Show("Opslaan mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                Console.WriteLine(i + " row(s) are affected in contactpersontype");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Edit contacttype: " + ex.Message);
            }
        }
        #endregion

        #region Public Vars
        public static ObservableCollection<ContactpersonType> lstContactTypes = GetContactTypes();
        #endregion
    }
}
