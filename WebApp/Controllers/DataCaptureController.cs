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
    public class DataCaptureController : Controller
    {
        [HttpGet]
        [Route("{brewStepId:int}")]
        public async Task<IEnumerable<DataCaptureValueDto>> Get(int brewStepId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                return await repo.GetDataCaptureValues(brewStepId);
            }
        }

        [HttpPost]
        public void Save([FromBody]DataCaptureValueDto[] values)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                repo.SaveDataCaptureValues(values);
                db.SaveChanges();
            }
        }

        [HttpGet]
        [Route("getDefinedValues/{brewId:int}")]
        public async Task<IEnumerable<DataCaptureValueDto>> GetDefinedDataCaptureValues(int brewId)
        {
            using (var db = new BrewMaticContext())
            {
                var repo = new BrewLogRepository(db);
                return await repo.GetDefinedDataCaptureValues(brewId);
            }
        }
    }
}
