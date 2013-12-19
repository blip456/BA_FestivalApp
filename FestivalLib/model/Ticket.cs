using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
//using FestivalLib.Model;
using Xceed.Wpf.Toolkit;

namespace FestivalLib.model
{
    public class Ticket
    {
        public static ObservableCollection<TicketType> lstTicketTypes = TicketType.GetTicketTypes();
        public static ObservableCollection<Ticket> lstAlleTickets = Ticket.GetTickets();
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

        public static Ticket GetTicketByID(int id)
        {
            Ticket gevondenTicket = new Ticket();
            foreach (Ticket ticket in lstAlleTickets)
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

        #region Methodes
        public static void PrintWord(Ticket ticket, Festival festival, string sPad)
        {
            string sFileNaam = ticket.ID + "_" + ticket.TicketHolder + ".docx";
            string sFullPad = sPad + "\\" + sFileNaam;
            try
            {
                File.Copy("template.docx", sFullPad, true);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
            
            WordprocessingDocument newDoc = WordprocessingDocument.Open(sFullPad, true);
            IDictionary<string, BookmarkStart> bookmarks = new Dictionary<string, BookmarkStart>();
            foreach (BookmarkStart bms in newDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>()) 
            {
                bookmarks[bms.Name] = bms;
            }

            //Festival name moet iets anders qua opmaak zijn
            Run runTitle = new Run(new Text(festival.Name + " - " + festival.Omschrijving));

            RunProperties propTitle = new RunProperties();
            RunFonts fontTitle = new RunFonts() { Ascii = "Segoe UI", HighAnsi = "Segoe UI" };
            FontSize sizeTitle = new FontSize() { Val = "36" };            

            propTitle.Append(fontTitle);
            propTitle.Append(sizeTitle);
            runTitle.PrependChild<RunProperties>(propTitle);

            bookmarks["FestivalTitle"].Parent.InsertAfter<Run>(runTitle, bookmarks["FestivalTitle"]);

            bookmarks["Name"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketHolder)), bookmarks["Name"]);
            bookmarks["Email"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketHolderEmail)), bookmarks["Email"]);
            bookmarks["Day"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Name)), bookmarks["Day"]);
            bookmarks["Type"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Categorie)), bookmarks["Type"]);
            bookmarks["Amount"].Parent.InsertAfter<Run>(new Run(new Text(ticket.Amount.ToString())), bookmarks["Amount"]);
            bookmarks["Price"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Price.ToString())), bookmarks["Price"]);
            double iTotalPrice = ticket.Amount * ticket.TicketType.Price;
            bookmarks["Totalprice"].Parent.InsertAfter<Run>(new Run(new Text(iTotalPrice.ToString())), bookmarks["Totalprice"]);

            //BARCODE TOEVOEGEN            
            //string code = Guid.NewGuid().ToString();
            string code = GenerateUnique(ticket.TicketHolderEmail);
            Run run = new Run(new Text(code));
            
            RunProperties prop = new RunProperties();
            RunFonts font = new RunFonts() { Ascii = "Free 3 of 9 Extended", HighAnsi = "Free 3 of 9 Extended" };
            FontSize size = new FontSize() { Val = "96" };

            prop.Append(font);
            prop.Append(size);
            run.PrependChild<RunProperties>(prop);

            bookmarks["Barcode"].Parent.InsertAfter<Run>(run, bookmarks["Barcode"]);
          
            newDoc.Close();
            MessageBox.Show(sFullPad + " is opgeslaan");
        }

        public static string GenerateUnique(string sEmail)
        {
            string ticks = DateTime.UtcNow.Ticks.ToString();
            string s1 = ticks.Substring(ticks.Length / 2, ticks.Length - (ticks.Length / 2));

            return s1;
        }
        #endregion
    }
}
