using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NET_CORE.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiTestController : ControllerBase
    {
        // GET: api/ApiTest
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApiTest/5
        [HttpGet]
        public string Get1(int id)
        {
            return "value";
        }

        // POST: api/ApiTest
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ApiTest/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
