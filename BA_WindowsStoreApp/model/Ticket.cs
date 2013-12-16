using FestivalLib.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Ticket
    {
        public static ObservableCollection<TicketType> lstTicketTypes = TicketType.GetTicketTypes();

        #region prop en field

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _ticketHolder;

        public string TicketHolder
        {
            get { return _ticketHolder; }
            set { _ticketHolder = value; }
        }

        private string _ticketHolderEmail;

        public string TicketHolderEmail
        {
            get { return _ticketHolderEmail; }
            set { _ticketHolderEmail = value; }
        }

        private TicketType _ticketType;

        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; }
        }

        private int _amount;

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #endregion

        public static int[] SoldTickets(ObservableCollection<Ticket> lst)
        {
            int iSoldNormal = 0;
            int iSoldVip = 0;
            foreach (var item in lst)
            {
                if (Convert.ToString(item.TicketType.Categorie) == "VIP")
                {
                    iSoldVip += item.Amount;
                }
                else if (Convert.ToString(item.TicketType.Categorie) == "Normaal")
                {
                    iSoldNormal += item.Amount;
                }
            }

            int[] arrSold = new int[2];
            arrSold[0] = iSoldVip;
            arrSold[1] = iSoldNormal;
            return arrSold;
        }

        #region SQL

        private static Ticket CreateTicket(DbDataReader reader)
        {
            Ticket ticket = new Ticket();
            ticket.ID = Convert.ToString(reader["ticket_id"]);
            ticket.TicketHolder = Convert.ToString(reader["ticket_name"]);
            ticket.TicketHolderEmail = Convert.ToString(reader["ticket_email"]);
            TicketType ticketType = new TicketType();
            ticketType = TicketType.GetTicketTypeByID(Convert.ToInt16(reader["tickettype_id"]));
            ticket.TicketType = ticketType;
            ticket.Amount = Convert.ToInt16(reader["ticket_amount"]);
   
            return ticket;
        }

        public static ObservableCollection<Ticket> GetTickets()
        {
            ObservableCollection<Ticket> lstTickets = new ObservableCollection<Ticket>();
            DbDataReader reader = Database.GetData("SELECT * FROM ticket");
            while (reader.Read())
            {
                lstTickets.Add(CreateTicket(reader));
            }
            return lstTickets;
        }

        public static Ticket GetTicketByID(ObservableCollection<Ticket> lst, int id)
        {
            Ticket gevondenTicket = new Ticket();
            foreach (Ticket ticket in lst)
            {
                if (ticket.ID == Convert.ToString(id))
                {
                    gevondenTicket = ticket;
                }
            }
            return gevondenTicket;
        }       

        public static string AddOrEdit(Ticket ticket)
        {
            string str = "";
            ObservableCollection<Ticket> lstTickets = GetTickets();
            if (lstTickets.Any(item => item.ID == ticket.ID))
            {
                EditTicket(ticket);
                str = "edit";
            }
            else
            {
                AddTicket(ticket);
                str = "add";
            }
            return str;
        }

        public static void AddTicket(Ticket ticket)
        {
            string sql = "INSERT INTO ticket(ticket_name, ticket_email, ticket_amount, tickettype_id) VALUES (@name, @email, @amount, @typeid);";

            DbParameter par1 = Database.AddParameter("@name", ticket.TicketHolder);
            DbParameter par2 = Database.AddParameter("@email", ticket.TicketHolderEmail);
            DbParameter par3 = Database.AddParameter("@amount", ticket.Amount);
            DbParameter par4 = Database.AddParameter("@typeid", ticket.TicketType.ID);

            int i = Database.ModifyData(sql, par1, par2, par3, par4);
           
            Console.WriteLine(i + " row(s) are affected ticket");
        }

        public static void EditTicket(Ticket ticket)
        {
            string sql = "UPDATE ticket SET ticket_name=@name, ticket_email=@email, ticket_amount=@amount WHERE ticket_id=@ID;";

            DbParameter par1 = Database.AddParameter("@name", ticket.TicketHolder);
            DbParameter par2 = Database.AddParameter("@email", ticket.TicketHolderEmail);
            DbParameter par3 = Database.AddParameter("@amount", ticket.Amount);

            DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(ticket.ID));

            int i = Database.ModifyData(sql, par1, par2, par3, parID);
            if (i == 0)
            {
                MessageBox.Show("Opslaan mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
            }
            Console.WriteLine(i + " row(s) are affected");
        }

        public static void DeleteTicket(int id)
        {
            string sql = "DELETE FROM ticket WHERE ticket_id = @ID;";

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
