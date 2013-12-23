using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            if (reader != null)
                reader.Close();
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

        public static void SendMail(Ticket ticket)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();

            try
            {
                MailAddress fromAddress = new MailAddress("admin@moustache.com", "Moustache Festival");

                // You can specify the host name or ipaddress of your server
                // Default in IIS will be localhost 
                smtpClient.Host = "localhost";

                //Default port will be 25
                smtpClient.Port = 25;

                //From address will be given as a MailAddress Object
                message.From = fromAddress;

                // To address collection of MailAddress
                message.To.Add(ticket.TicketHolderEmail);
                message.Subject = "Moustache Festival - Ticket";

                //Body can be Html or text format
                //Specify true if it  is html message
                message.IsBodyHtml = true;                

                // Message body content
                message.Body = "<html><head><title></title><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=320, target-densitydpi=device-dpi'><style type='text/css'>/* Mobile-specific Styles */@media only screen and (max-width: 660px){table[class=w0], td[class=w0]{width: 0 !important;}table[class=w10], td[class=w10], img[class=w10]{width:10px !important;}table[class=w15], td[class=w15], img[class=w15]{width:5px !important;}table[class=w30], td[class=w30], img[class=w30]{width:10px !important;}table[class=w60], td[class=w60], img[class=w60]{width:10px !important;}table[class=w125], td[class=w125], img[class=w125]{width:80px !important;}table[class=w130], td[class=w130], img[class=w130]{width:55px !important;}table[class=w140], td[class=w140], img[class=w140]{width:90px !important;}table[class=w160], td[class=w160], img[class=w160]{width:180px !important;}table[class=w170], td[class=w170], img[class=w170]{width:100px !important;}table[class=w180], td[class=w180], img[class=w180]{width:80px !important;}table[class=w195], td[class=w195], img[class=w195]{width:80px !important;}table[class=w220], td[class=w220], img[class=w220]{width:80px !important;}table[class=w240], td[class=w240], img[class=w240]{width:180px !important;}table[class=w255], td[class=w255], img[class=w255]{width:185px !important;}table[class=w275], td[class=w275], img[class=w275]{width:135px !important;}table[class=w280], td[class=w280], img[class=w280]{width:135px !important;}table[class=w300], td[class=w300], img[class=w300]{width:140px !important;}table[class=w325], td[class=w325], img[class=w325]{width:95px !important;}table[class=w360], td[class=w360], img[class=w360]{width:140px !important;}table[class=w410], td[class=w410], img[class=w410]{width:180px !important;}table[class=w470], td[class=w470], img[class=w470]{width:200px !important;}table[class=w580], td[class=w580], img[class=w580]{width:280px !important;}table[class=w640], td[class=w640], img[class=w640]{width:300px !important;}table[class*=hide], td[class*=hide], img[class*=hide], p[class*=hide], span[class*=hide]{display:none !important;}table[class=h0], td[class=h0]{height: 0 !important;}p[class=footer-content-left]{text-align: center !important;}#headline p{font-size: 30px !important;}.article-content, #left-sidebar{-webkit-text-size-adjust: 90% !important;-ms-text-size-adjust: 90% !important;}.header-content, .footer-content-left{-webkit-text-size-adjust: 80% !important;-ms-text-size-adjust: 80% !important;}img{height: auto;line-height: 100%;}}/* Client-specific Styles */#outlook a{padding: 0;}/* Force Outlook to provide a 'view in browser' button. */body{width: 100% !important;}.ReadMsgBody{width: 100%;}.ExternalClass{width: 100%;display:block !important;}/* Force Hotmail to display emails at full width *//* Reset Styles *//* Add 100px so mobile switch bar doesn't cover street address. */body{background-color: #EAEAEC;margin: 0;padding: 0;}img{outline: none;text-decoration: none;display: block;}br, strong br, b br, em br, i br{line-height:100%;}h1, h2, h3, h4, h5, h6{line-height: 100% !important;-webkit-font-smoothing: antialiased;}h1 a, h2 a, h3 a, h4 a, h5 a, h6 a{color: blue !important;}h1 a:active, h2 a:active, h3 a:active, h4 a:active, h5 a:active, h6 a:active{color: red !important;}/* Preferably not the same color as the normal header link color. There is limited support for psuedo classes in email clients, this was added just for good measure. */h1 a:visited, h2 a:visited, h3 a:visited, h4 a:visited, h5 a:visited, h6 a:visited{color: purple !important;}/* Preferably not the same color as the normal header link color. There is limited support for psuedo classes in email clients, this was added just for good measure. */ table td, table tr{border-collapse: collapse;}.yshortcuts, .yshortcuts a, .yshortcuts a:link,.yshortcuts a:visited, .yshortcuts a:hover, .yshortcuts a span{color: black;text-decoration: none !important;border-bottom: none !important;background: none !important;}/* Body text color for the New Yahoo. This example sets the font of Yahoo's Shortcuts to black. *//* This most probably won't work in all email clients. Don't include code blocks in email. */code{white-space: normal;word-break: break-all;}#background-table{background-color: #EAEAEC;}/* Webkit Elements */#top-bar{border-radius:6px 6px 0px 0px;-moz-border-radius: 6px 6px 0px 0px;-webkit-border-radius:6px 6px 0px 0px;-webkit-font-smoothing: antialiased;background-color: #FF9619;color: #FF9619;}#top-bar a{font-weight: bold;color: #FF9619;text-decoration: none;}#footer{border-radius:0px 0px 6px 6px;-moz-border-radius: 0px 0px 6px 6px;-webkit-border-radius:0px 0px 6px 6px;-webkit-font-smoothing: antialiased;}/* Fonts and Content */body, td{font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;}.header-content, .footer-content-left, .footer-content-right{-webkit-text-size-adjust: none;-ms-text-size-adjust: none;}/* Prevent Webkit and Windows Mobile platforms from changing default font sizes on header and footer. */.header-content{font-size: 12px;color: #FF9619;}.header-content a{font-weight: bold;color: #FF9619;text-decoration: none;}#headline p{color: #ffffff;font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;font-size: 48px;text-align: center;margin-top:0px;margin-bottom:30px;}#headline p a{color: #ffffff;text-decoration: none;}.article-title{font-size: 18px;line-height:24px;color: #e97900;font-weight:bold;margin-top:0px;margin-bottom:18px;font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;}.article-title a{color: #e97900;text-decoration: none;}.article-title.with-meta{margin-bottom: 0;}.article-meta{font-size: 13px;line-height: 20px;color: #ccc;font-weight: bold;margin-top: 0;}.article-content{font-size: 13px;line-height: 18px;color: #444444;margin-top: 0px;margin-bottom: 18px;font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif;}.article-content a{color: #e97900;font-weight:bold;text-decoration:none;}.article-content img{max-width: 100%}.article-content ol, .article-content ul{margin-top:0px;margin-bottom:18px;margin-left:19px;padding:0;}.article-content li{font-size: 13px;line-height: 18px;color: #444444;}.article-content li a{color: #e97900;text-decoration:underline;}.article-content p{margin-bottom: 15px;}.footer-content-left{font-size: 12px;line-height: 15px;color: #ffffff;margin-top: 0px;margin-bottom: 15px;}.footer-content-left a{color: #fffffc;font-weight: bold;text-decoration: none;}.footer-content-right{font-size: 11px;line-height: 16px;color: #ffffff;margin-top: 0px;margin-bottom: 15px;}.footer-content-right a{color: #fffffc;font-weight: bold;text-decoration: none;}#footer{background-color: #FF9619;color: #ffffff;}#footer a{color: #fffffc;text-decoration: none;font-weight: bold;}#permission-reminder{white-space: normal;}#street-address{color: #ffffff;white-space: normal;}</style><!--[if gte mso 9]><style _tmplitem='511' >.article-content ol, .article-content ul{margin: 0 0 0 24px;padding: 0;list-style-position: inside;}</style><![endif]--></head><body><table width='100%' cellpadding='0' cellspacing='0' border='0' id='background-table'><tbody><tr><td align='center' bgcolor='#EAEAEC'><table class='w640' style='margin:0 10px;' width='640' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w640' width='640' height='20'></td></tr><tr><td class='w640' width='640'><table id='top-bar' class='w640' width='640' cellpadding='0' cellspacing='0' border='0' bgcolor='#FF9619'><tbody><tr><td class='w15' width='15'></td><td class='w325' width='350' valign='middle' align='left'><table class='w325' width='350' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w325' width='350' height='8'></td></tr></tbody></table><table class='w325' width='350' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w325' width='350' height='8'></td></tr></tbody></table></td><td class='w30' width='30'></td><td class='w255' width='255' valign='middle' align='right'><table class='w255' width='255' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w255' width='255' height='8'></td></tr></tbody></table><table cellpadding='0' cellspacing='0' border='0'><tbody><tr></tr></tbody></table><table class='w255' width='255' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w255' width='255' height='8'></td></tr></tbody></table></td><td class='w15' width='15'></td></tr></tbody></table></td></tr><tr><td id='header' class='w640' width='640' align='center' bgcolor='#FF9619'><table class='w640' width='640' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w30' width='30'></td><td class='w580' width='580' height='30'></td><td class='w30' width='30'></td></tr><tr><td class='w30' width='30'></td><td class='w580' width='580'><div align='center' id='headline'><p><strong><singleline label='Title'>Moustache Festival</singleline></strong></p></div></td><td class='w30' width='30'></td></tr></tbody></table></td></tr><tr><td class='w640' width='640' height='30' bgcolor='#ffffff'></td></tr><tr id='simple-content-row'><td class='w640' width='640' bgcolor='#ffffff'><table class='w640' width='640' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w30' width='30'></td><td class='w580' width='580'><repeater><layout label='Text only'><table class='w580' width='580' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w580' width='580'><p align='left' class='article-title'><singleline label='Title'>Proficiat met uw ticket</singleline></p><div align='left' class='article-content'><multiline label='Description'>Moustache Festival wilt u feliciteren met de aankoop van uw ticket. U hebt de juiste keuze gemaakt om aanwezig te zijn op het grootste festival van België.<br><br>Hieronder vindt u uw gegevens terug. Gelieve deze te controleren.</multiline></div></td></tr><tr><td class='w580' width='580' height='10'></td></tr></tbody></table></layout><layout label='Two columns'><table class='w580' width='580' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w275' width='275' valign='top'><table class='w275' width='275' cellpadding='0' cellspacing='0' border='0'><tbody><tr><td class='w275' width='275'><p align='left' class='article-title'><singleline label='Title'>Gegevens</singleline></p><div align='left' class='article-content'><multiline label='Description'>"+ticket.TicketHolder+"</multiline></div><div align='left' class='article-content'><multiline label='Description'>"+ticket.TicketHolderEmail+"</multiline></div><div align='left' class='article-content'><multiline label='Description'>"+ticket.Amount+" tickets</multiline></div><div align='left' class='article-content'><multiline label='Description'>"+ticket.TicketType.NameCat+" (€"+ticket.TicketType.Price+") "+"</multiline></div></td></tr><tr><td class='w275' width='275' height='10'></td></tr></tbody></table></td><td class='w30' width='30'></td><td class='w275' width='275' valign='top'></td></tr></tbody></table></layout></repeater></td><td class='w30' width='30'></td></tr></tbody></table></td></tr><tr><td class='w640' width='640' height='15' bgcolor='#ffffff'></td></tr><tr><td class='w640' width='640'><table id='footer' class='w640' width='640' cellpadding='0' cellspacing='0' border='0' bgcolor='#FF9619'><tbody><tr><td class='w30' width='30'></td><td class='w580 h0' width='360' height='30'></td><td class='w0' width='60'></td><td class='w0' width='160'></td><td class='w30' width='30'></td></tr><tr><td class='w30' width='30'></td><td class='w580' width='360' valign='top'><span class='hide'><p id='permission-reminder' align='left' class='footer-content-left'><span>Indien u geen tickets besteld hebt gelieve ons dan te mailen</span></p></span></td></tr><tr><td class='w30' width='30'></td><td class='w580 h0' width='360' height='15'></td><td class='w0' width='60'></td><td class='w0' width='160'></td><td class='w30' width='30'></td></tr></tbody></table></td></tr><tr><td class='w640' width='640' height='60'></td></tr></tbody></table></td></tr></tbody></table></body></html>";
                //message.Body = "lolol";
                // Send SMTP mail
                smtpClient.Send(message);

                //lblStatus.Text = "Email successfully sent.";
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Send Email Failed." + ex.Message;
            }
        }
        #endregion
    }
}
