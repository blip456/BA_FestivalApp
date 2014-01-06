using BA_RestService.Models._DAL;
using FestivalLibPort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BA_RestService.Controllers
{
    public class GenreController : ApiController
    {
        // GET api/genre
        public IEnumerable<Genre> Get()
        {
            return GenreRepository.Getgenres();
        }

        // GET api/genre/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/genre
        public void Post([FromBody]string value)
        {
        }

        // PUT api/genre/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/genre/5
        public void Delete(int id)
        {
        }
    }
}
