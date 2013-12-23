using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BA_RestService.Controllers
{
    public class FestivalController : ApiController
    {
        // GET api/festival
        public Festival Get()
        {
            return Models._DAL.FestivalRepository.GetFestivals();
        }

        // GET api/festival/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/festival
        public void Post([FromBody]string value)
        {
        }

        // PUT api/festival/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/festival/5
        public void Delete(int id)
        {
        }
    }
}
