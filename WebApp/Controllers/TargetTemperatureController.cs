using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class TargetTemperatureController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<BrewTargetTemperature> Get()
        {
            //todo! Return async data!!
            var logs = new List<BrewTempLog>();
            using (var db = new BrewMaticContext())
            {
                var t = await db.TargetTemp.FirstOrDefaultAsync();
                if (t != null)
                    return t;
                t = new BrewTargetTemperature { Target1 = 20, Target2 = 20 };
                db.Add(t);
                await db.SaveChangesAsync();
                return t;
            }
        }


        // POST api/values
        [HttpPost]
        public async void Post([FromBody]BrewTargetTemperature value)
        {
            using (var db = new BrewMaticContext())
            {
                var t = await db.TargetTemp.FirstOrDefaultAsync();
                if (t == null)
                {
                    t = new BrewTargetTemperature();
                    db.Add(t);
                }
                t.Target1 = value.Target1;
                t.Target2 = value.Target2;
                await db.SaveChangesAsync();
            }
        }
    }
}
