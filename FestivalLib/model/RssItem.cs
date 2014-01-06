﻿using System;
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
        #region Props, Fields and Constructor
        public RssItem()
        {

        }

        public string Link { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public DateTime Date { get; set; }
        #endregion

        #region SQL
        //dez haalt aale rss feed items op uit de DB
        public static List<SyndicationItem> GetRssItems()
        {
            try
            {
                List<SyndicationItem> lstFeedItems = new List<SyndicationItem>();
                DbDataReader reader = Database.GetData("SELECT * FROM rssfeed;");

                while (reader.Read())
                {                    
                    SyndicationItem item = new SyndicationItem((string)reader["rssfeed_title"], (string)reader["rssfeed_descr"], new Uri((string)reader["rssfeed_link"]), Convert.ToString(reader["rssfeed_id"]), (DateTime)reader["rssfeed_date"]);
                    lstFeedItems.Add(item);
                }
                if (reader != null)
                    reader.Close();
                return lstFeedItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine("get rss items: " + ex.Message);
                return null;
            }
        }

        //een nieuw rss post toevoegen aan de DB
        public static void AddRssItem(RssItem item)
        {
            try
            {
                string sql = "INSERT INTO rssfeed(rssfeed_link, rssfeed_descr, rssfeed_title, rssfeed_date) VALUES (@link, @descr, @title, @date);";

                DbParameter par1 = Database.AddParameter("@link", item.Link);
                DbParameter par2 = Database.AddParameter("@descr", item.Descr);
                DbParameter par3 = Database.AddParameter("@title", item.Title);
                DbParameter par4 = Database.AddParameter("@date", DateTime.Now);

                int i = Database.ModifyData(sql, par1, par2, par3, par4);

                Console.WriteLine(i + " row(s) are affected ticket");
            }
            catch (Exception ex)
            {
                Console.WriteLine("add rss item: " + ex.Message);
            }
        }
        #endregion
    }
}
