using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic;
using WebApp.Model;
using WebApp.Model.BrewGuide;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class BrewGuideController : Controller
    {
        [HttpGet]
        [Route("{brewId:int}")]
        public async Task<CurrentBrewDto> Get(int brewId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                return await repo.GetBrewDto(brewId);
            }
        }

        [HttpGet]
        [Route("getBrewHistory/{brewId:int}")]
        public async Task<IEnumerable<BrewLogHistoryDto>> GetBrewHistory(int brewId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                return await repo.GetBrewHistory(brewId);
            }
        }

        [HttpPost]
        [Route("goToNextStep")]
        public void GoToNextStep([FromBody]int brewId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                repo.GoToNextStep(brewId);
                db.SaveChanges();
            }
        }

        [HttpPost]
        [Route("goBackOneStep")]
        public void GoToPreviousStep([FromBody]int brewId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                repo.GoBackOneStep(brewId);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("getLatest")]
        public async Task<int> GetLatestBrew()
        {
            using (var db = new BrewMaticContext())
            {
                return await db.BrewLogs.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
            }
        }

        [HttpGet]
        [Route("GetDefaultSetup")]
        public SetupDto GetDefault()
        {
            return new SetupDto
            {
                MashTemp = 67.0f,
                StrikeTemp = 73.6f,
                SpargeTemp = 75.6f,
                MashOutTemp = 78.0f,
                MashTimeInMinutes = 60,
                BoilTimeInMinutes = 60,
            };
        }

        [HttpPost]
        [Route("StartNewBrew")]
        public async Task<int> StartNewBrew([FromBody]SetupDto value)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);

                var brewStep = repo.InitializeNewBrew(value);

                await db.SaveChangesAsync();
                return brewStep.BrewLog.Id;
            }
        }


    }
}
