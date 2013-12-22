using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ServiceModel.Syndication;
using System.Data.Common;
using FestivalLib.model;

namespace SSA_FestivalApp.Controllers
{
    public class RSSController : ApiController
    {
        // GET api/rss
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public Rss20FeedFormatter Get()
        {
            var feed = new SyndicationFeed("Moustache Festival", "Official Moustache Festival RSS feed ", new Uri("http://localhost:61968/"));
            feed.Authors.Add(new SyndicationPerson("Admin"));
            feed.Categories.Add(new SyndicationCategory("News"));
            feed.Description = new TextSyndicationContent("Keep up to date with Moustache Festival");

            //items moeten nog uit DB komen
            

            SyndicationItem item1 = new SyndicationItem("Item One", "Content voor eerste item", new Uri("http://localhost/Ticket/Reserveer"), "ID", DateTime.Now);

            List<SyndicationItem> lstItems = new List<SyndicationItem>();
            lstItems = RssItem.GetRssItems();
            feed.Items = lstItems;
            return new Rss20FeedFormatter(feed);
        }

        // GET api/rss/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/rss
        public void Post([FromBody]string value)
        {
        }

        // PUT api/rss/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/rss/5
        public void Delete(int id)
        {
        }

       
    }
}
