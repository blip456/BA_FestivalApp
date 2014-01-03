using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using Xceed.Wpf.Toolkit;
//using FestivalLib.Model;

namespace FestivalLib.model
{
    public class TicketType
    {
        public TicketType()
        {

        }

        #region field en prop

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

        private double _price;

        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _availableTickets;

        public int AvailableTickets
        {
            get { return _availableTickets; }
            set { _availableTickets = value; }
        }

        private string _categorie;

        public string Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }

        private string _nameCat;

        public string NameCat
        {
            get { return _nameCat; }
            set { _nameCat = value; }
        }

        #endregion

        #region SQL
        private static TicketType CreateTicket(DbDataReader reader)
        {
            try
            {
                TicketType ticket = new TicketType();
                ticket.ID = Convert.ToString(reader["tickettype_id"]);
                ticket.Name = Convert.ToString(reader["tickettype_name"]);
                ticket.Price = Convert.ToDouble(reader["tickettype_price"]);
                ticket.AvailableTickets = Convert.ToInt32(reader["tickettype_available"]);
                ticket.Categorie = Convert.ToString(reader["tickettype_categorie"]);
                ticket.NameCat = ticket.Name + " - " + ticket.Categorie;
                return ticket;
            }
            catch (Exception ex)
            {
                Console.WriteLine("create tickettype: " + ex.Message);
                return null;
            }
        }

        public static ObservableCollection<TicketType> GetTicketTypes()
        {
            try
            {
                ObservableCollection<DateTime> lstAantalDagen = Festival.aantalDagen();
                int iAantalDagen = lstAantalDagen.Count * 2;

                ObservableCollection<TicketType> lstTicketTypes = new ObservableCollection<TicketType>();
                DbDataReader reader = Database.GetData("SELECT TOP " + iAantalDagen + " * FROM tickettype");
                while (reader.Read())
                {
                    lstTicketTypes.Add(CreateTicket(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstTicketTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get ticket types: " + ex.Message);
                return null;
            }

        }

        public static ObservableCollection<TicketType> GetTicketTypeByString(string search)
        {
            try
            {
                ObservableCollection<DateTime> lstAantalDagen = Festival.aantalDagen();
                int iAantalDagen = lstAantalDagen.Count;

                ObservableCollection<TicketType> lstGevondenTicketTypes = new ObservableCollection<TicketType>();
                string sql = "SELECT TOP " + iAantalDagen + " * FROM tickettype WHERE tickettype_categorie LIKE @str;";
                DbParameter par1 = Database.AddParameter("@i", iAantalDagen);
                DbParameter par2 = Database.AddParameter("@str", "%" + search + "%");
                DbDataReader reader = Database.GetData(sql, par1, par2);
                while (reader.Read())
                {
                    lstGevondenTicketTypes.Add(CreateTicket(reader));
                }
                if (reader != null)
                    reader.Close();
                return lstGevondenTicketTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get ticket types bystring: " + ex.Message);
                return null;
            }

        }

        public static TicketType GetTicketTypeByID(int id)
        {
            try
            {
                TicketType gevondenTicketType = new TicketType();
                foreach (TicketType ticketType in lstAlleTypes)
                {
                    if (ticketType.ID == Convert.ToString(id))
                    {
                        gevondenTicketType = ticketType;
                    }
                }
                return gevondenTicketType;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get tickettype by id: " + ex.Message);
                return null;
            }
        }

        public static int CountTotalNormal(ObservableCollection<TicketType> lst)
        {
            try
            {
                int iTotal = 0;
                foreach (var item in GetTicketTypeByString("Normaal"))
                {
                    iTotal += item.AvailableTickets;
                }
                return iTotal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("count total normal: " + ex.Message);
                return 0;
            }
        }

        public static int CountTotalVip(ObservableCollection<TicketType> lst)
        {
            try
            {
                int iTotal = 0;
                foreach (var item in GetTicketTypeByString("VIP"))
                {
                    iTotal += item.AvailableTickets;
                }
                return iTotal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("count totl vip: " + ex.Message);
                return 0;
            }

        }

        public static void EditTicketType(TicketType ticket)
        {
            try
            {
                string sql = "UPDATE tickettype SET tickettype_name=@name, tickettype_price=@price, tickettype_available=@available WHERE tickettype_id=@ID;";

                DbParameter par1 = Database.AddParameter("@name", ticket.Name);
                DbParameter par2 = Database.AddParameter("@price", ticket.Price);
                DbParameter par3 = Database.AddParameter("@available", ticket.AvailableTickets);

                DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(ticket.ID));

                int i = Database.ModifyData(sql, par1, par2, par3, parID);
                Console.WriteLine(i + " row(s) are affected");
                if (i == 1)
                {
                    MessageBox.Show("Opslaan is gelukt", "Gelukt", System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Opslaan mislukt", "Mislukt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("edit ticket type: " + ex.Message);
            }


        }
        #endregion

        #region Public Vars
        public static ObservableCollection<TicketType> lstAlleTypes = GetTicketTypes();
        #endregion
    }
}
