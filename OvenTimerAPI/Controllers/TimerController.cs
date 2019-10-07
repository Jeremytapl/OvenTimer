using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OvenTimerAPI.Services;

namespace OvenTimerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimerController : ControllerBase
    {
        private IConfiguration _config;
        private TimerService _tSvc;

        public TimerController(IConfiguration configuration)
        {
            _config = configuration;
            _tSvc = new TimerService(new TimerContext(configuration));
        }

        [HttpGet]
        public ActionResult<TimeSetViewModel> Get()
        {
            return _tSvc.GetTimeSets();
        }

        [HttpPatch]
        public void Patch([FromBody]TimeSet timeSet) 
        {
            _tSvc.UpdateTimeSet(timeSet);
        }

        [HttpPost]
        public void Post([FromBody]TimeSetViewModel timeSets)
        {
            _tSvc.AddTimeSets(timeSets);
        }
    }
}
