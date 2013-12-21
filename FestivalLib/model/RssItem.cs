using System;
using System.Collections.Generic;
using System.Data;
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

        public static List<RssItem> GetRssItems()
        {
            Rss20FeedFormatter feedFormatter = new Rss20FeedFormatter();
            XmlReader rssReader = XmlReader.Create("http://localhost:61968/api/rss");
            if (feedFormatter.CanRead(rssReader))
            {
                feedFormatter.ReadFrom(rssReader);
                rssReader.Close();
            }


            return null;


            //string rssURL = "http://localhost:61968/api/rss";
            //List<RssItem> rssFeeds = new List<RssItem>();
            //try
            //{
            //    XDocument doc = new XDocument();
            //    doc = XDocument.Load(rssURL);            
            //    var items = (from x in doc.Descendants("item")
            //                 select new
            //                 {
            //                    title = x.Element("title").Value,
            //                    link =x.Element("link").Value,
            //                    descr = x.Element("description").Value
            //                 });
            //    if (items != null)
            //    {
            //        foreach (var i in items)
            //        {
            //            RssItem rssItem = new RssItem();
            //            rssItem.Title = i.title;
            //            rssItem.Link = i.link;
            //            rssItem.Descr = i.descr;
            //            rssFeeds.Add(rssItem);
            //        }
            //        return rssFeeds;
            //    }
            //}                
            //catch (Exception ex)
            //{
                
            //    throw;
            //}

            //return rssFeeds;
        }
    }
}
