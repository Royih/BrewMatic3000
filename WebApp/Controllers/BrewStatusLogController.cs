using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class BrewStatusLogController : Controller
    {
        // GET api/values
        [HttpGet]
        [Route("get50Latest")]
        public async Task<IEnumerable<BrewTempLog>> Get50Latest()
        {
            using (var db = new BrewMaticContext())
            {
                return await db.TempLogs.OrderByDescending(x => x.Id).Take(50).ToListAsync();
            }
        }


        [HttpGet]
        public async Task<BrewTempLog> GetLatest()
        {
            using (var db = new BrewMaticContext())
            {
                var l = await db.TempLogs.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (l != null)
                {
                    return l;
                }
                var newLog = new BrewTempLog
                {
                    Temp1 = 0,
                    Temp2 = 0,
                    Heater1Percentage = 0,
                    Heater2Percentage = 0,
                    TimeStamp = DateTime.Now
                };
                db.TempLogs.Add(newLog);
                await db.SaveChangesAsync();
                return newLog;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
