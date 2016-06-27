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
    public class StateController : Controller
    {

        private static DateTime _started = DateTime.Now;
        private static string _stateName = "Ready";

        // GET api/values
        [HttpGet]
        [Route("GetWhenStarted")]
        public DateTime GetWhenStarted()
        {
            return _started;
        }

        [HttpPost]
        [Route("setStateName/{newStateName}")]
        public void SetStateName(string newStateName)
        {
            _stateName = newStateName;
        }

        [HttpGet]
        [Route("getStateName")]
        public string GetStateName()
        {
            return _stateName;
        }
    }
}
