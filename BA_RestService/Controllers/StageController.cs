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
    public class StageController : ApiController
    {
        // GET api/stage
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/stage/5
        public IEnumerable<Stage> Get(string id)
        {
            DateTime date = Convert.ToDateTime(id);
            return StageRepository.GetStagesByDay(date);
        }

        // POST api/stage
        public void Post([FromBody]string value)
        {
        }

        // PUT api/stage/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/stage/5
        public void Delete(int id)
        {
        }
    }
}
