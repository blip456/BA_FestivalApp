using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FestivalLib.model
{
    public class RssItem
    {
        public RssItem()
        {

        }

        public string Link { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public DateTime Date { get; set; }


        public static List<SyndicationItem> GetRssItems()
        {
            List<SyndicationItem> lstFeedItems = new List<SyndicationItem>();
            DbDataReader reader = Database.GetData("SELECT * FROM rssfeed;");

            while (reader.Read())
            {
                SyndicationItem item = new SyndicationItem((string)reader["rssfeed_title"], (string)reader["rssfeed_descr"], new Uri((string)reader["rssfeed_link"]), "ID", (DateTime)reader["rssfeed_date"]);
                lstFeedItems.Add(item);
            }           
            return lstFeedItems;
        }

        public static void AddRssItem(RssItem item)
        {
            string sql = "INSERT INTO rssfeed(rssfeed_link, rssfeed_descr, rssfeed_title, rssfeed_date) VALUES (@link, @descr, @title, @date);";

            DbParameter par1 = Database.AddParameter("@link", item.Link);
            DbParameter par2 = Database.AddParameter("@descr", item.Descr);
            DbParameter par3 = Database.AddParameter("@title", item.Title);
            DbParameter par4 = Database.AddParameter("@date", DateTime.Now);

            int i = Database.ModifyData(sql, par1, par2, par3, par4);

            Console.WriteLine(i + " row(s) are affected ticket");
        }
    }
}
