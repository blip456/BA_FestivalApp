using BA_RestService.Models._DAL;
using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BA_RestService.Controllers
{
    public class LineUpController : ApiController
    {
        // GET api/lineup
        public IEnumerable<LineUp> Get()
        {
            return LineUpRepository.GetLineUps();
        }

        // GET api/lineup/5
        public IEnumerable<LineUp> Get(string id)
        {
            DateTime date = Convert.ToDateTime(id);
            return LineUpRepository.GetBandsByDate(date);
        }

        // POST api/lineup
        public void Post([FromBody]string value)
        {
        }

        // PUT api/lineup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/lineup/5
        public void Delete(int id)
        {
        }
    }
}
