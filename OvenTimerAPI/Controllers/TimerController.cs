using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OvenTimerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimerController : ControllerBase
    {
        private IConfiguration _config;

        public TimerController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public string Get()
        {
            string comPort = _config["ComPort"];

            using(var arduino = new Arduino(comPort))
            {
                return arduino.Read();
            }
        }
    }
}
