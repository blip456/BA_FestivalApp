using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using Xceed.Wpf.Toolkit;
//using FestivalLib.Model;

namespace FestivalLib.model
{
    public class TicketType
    {
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
        


        #endregion

        public static ObservableCollection<TicketType> lstAlleTypes = GetTicketTypes();

        #region SQL
        private static TicketType CreateTicket(DbDataReader reader)
        {
            TicketType ticket = new TicketType();
            ticket.ID = Convert.ToString(reader["tickettype_id"]);
            ticket.Name = Convert.ToString(reader["tickettype_name"]);
            ticket.Price = Convert.ToDouble(reader["tickettype_price"]);
            ticket.AvailableTickets = Convert.ToInt32(reader["tickettype_available"]);
            ticket.Categorie = Convert.ToString(reader["tickettype_categorie"]);
            return ticket;
        }

        public static ObservableCollection<TicketType> GetTicketTypes()
        {
            ObservableCollection<TicketType> lstTicketTypes = new ObservableCollection<TicketType>();
            DbDataReader reader = Database.GetData("SELECT * FROM tickettype");
            while (reader.Read())
            {
                lstTicketTypes.Add(CreateTicket(reader));
            }
            return lstTicketTypes;
        }

        //public static ObservableCollection<TicketType> GetTicketTypeByString(string search)
        //{
        //    ObservableCollection<TicketType> lstGevondenTicketTypes = new ObservableCollection<TicketType>();
        //    DbParameter par = Database.AddParameter("@str", "%" + search + "%");
        //    DbDataReader reader = Database.GetData("SELECT * FROM tickettype WHERE tickettype_categorie LIKE @str;", par);
        //    while (reader.Read())
        //    {
        //        lstGevondenTicketTypes.Add(CreateTicket(reader));
        //    }

        //    return lstGevondenTicketTypes;
        //}

        public static ObservableCollection<TicketType> GetTicketTypeByString(string search)
        {
            ObservableCollection<TicketType> lstGevondenTickets = new ObservableCollection<TicketType>();
            foreach (TicketType ticket in lstAlleTypes)
            {
                if (ticket.Categorie.ToUpper().Contains(search.ToUpper()))
                {
                    lstGevondenTickets.Add(ticket);
                }
            }
            return lstGevondenTickets;
        }

        //public static TicketType GetTicketByID(int id)
        //{
        //    TicketType gevondenTicket = new TicketType();
        //    DbParameter par = Database.AddParameter("@ID", id);
        //    DbDataReader reader = Database.GetData("SELECT * FROM tickettype WHERE tickettype_id=@ID", par);
        //    while (reader.Read())
        //    {
        //        gevondenTicket = CreateTicket(reader);
        //        return gevondenTicket;
        //    }

        //    return gevondenTicket;
        //}

        public static TicketType GetTicketTypeByID( int id)
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

        
        public static int CountTotalNormal(ObservableCollection<TicketType> lst)
        {
            int iTotal = 0;
            foreach (var item in GetTicketTypeByString("Normaal"))
            {
                iTotal += item.AvailableTickets;
            }
            return iTotal;
        }

        public static int CountTotalVip(ObservableCollection<TicketType> lst)
        {
            int iTotal = 0;
            foreach (var item in GetTicketTypeByString("VIP"))
            {
                iTotal += item.AvailableTickets;
            }
            return iTotal;
        }

        public static void EditTicketType(TicketType ticket)
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

        #endregion
    }
}
