
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic;
using WebApp.Model;
using WebApp.Model.BrewLogs;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class BrewLogController : Controller
    {
        [HttpGet]
        public IEnumerable<BrewLogsDto> Get()
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                return repo.ListBrewLogs().ToList();
            }
        }
    }
}
