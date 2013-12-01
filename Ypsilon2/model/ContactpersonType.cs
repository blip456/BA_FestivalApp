using Ypsilon2.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;

namespace Ypsilon2.model
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
        private static ContactpersonType CreateContactType(DbDataReader reader)
        {
            ContactpersonType contactType = new ContactpersonType();
            contactType.ID = Convert.ToString(reader["contactpersontype_id"]);
            contactType.Name = Convert.ToString(reader["contactpersontype_name"]);            
            return contactType;
        }

        public static ObservableCollection<ContactpersonType> GetContactTypes()
        {
            ObservableCollection<ContactpersonType> lstContactTypes = new ObservableCollection<ContactpersonType>();
            DbDataReader reader = Database.GetData("SELECT * FROM contactpersontype");
            while (reader.Read())
            {
                lstContactTypes.Add(CreateContactType(reader));
            }
            return lstContactTypes;
        }

        //public static ContactpersonType GetContactTypeByID(int id)
        //{
        //    ContactpersonType gevondenContactType = new ContactpersonType();
        //    DbParameter par = Database.AddParameter("@ID", id);
        //    DbDataReader reader = Database.GetData("SELECT * FROM contactpersontype WHERE id=@ID", par);
        //    while (reader.Read())
        //    {
        //        gevondenContactType = CreateContactType(reader);
        //        return gevondenContactType;
        //    }

        //    return gevondenContactType;
        //}

        public static ContactpersonType GetContactTypeByID(ObservableCollection<ContactpersonType> lst, int id)
        {
            ContactpersonType gevondenContactType = new ContactpersonType();
            foreach (ContactpersonType contactType in lst)
            {
                if (contactType.ID == Convert.ToString(id))
                {
                    gevondenContactType = contactType;
                }
            }
            return gevondenContactType;
        }

        public static string AddOrEdit(ContactpersonType contactType)
        {
            string str = "";
            ObservableCollection<ContactpersonType> lstContactTypes = GetContactTypes();
            if (lstContactTypes.Any(item => item.ID == contactType.ID))
            {
                EditContactType(contactType);
                str = "edit";
            }
            else
            {
                AddContactType(contactType);
                str = "add";
            }
            return str;
        }

        public static void AddContactType(ContactpersonType contactType)
        {
            string sql = "INSERT INTO contactpersontype(contactpersontype_name) VALUES (@name);";

            DbParameter par1 = Database.AddParameter("@name", contactType.Name);            

            int i = Database.ModifyData(sql, par1);
            Console.WriteLine(i + " row(s) are affected in contactpersontype");
        }

        public static void EditContactType(ContactpersonType contactType)
        {
            string sql = "UPDATE contactpersontype SET contactpersontype_name=@name WHERE contactpersontype_id=@ID;";

            DbParameter par1 = Database.AddParameter("@name", contactType.Name); 

            DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(contactType.ID));

            int i = Database.ModifyData(sql, par1, parID);
            Console.WriteLine(i + " row(s) are affected in contactpersontype");
        }
        #endregion

    }
}
