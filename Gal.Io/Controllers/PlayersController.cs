using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using League_Recorder_Backend.Interfaces;
using League_Recorder_Backend.Interfaces.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace League_Recorder_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<PlayersController> _logger;
        private readonly IRiotService _riotService;
        public PlayersController(IConfiguration config, ILogger<PlayersController> logger, IRiotService riotService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
        }

        // GET api/players
        [HttpGet]
        public ActionResult<IEnumerable<SummonerDTO>> Get()
        {
            var response = _riotService.GetSummonerByName("Kuenaimaku");
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet("{name}")]
        public ActionResult <SummonerDTO> Validate(string name)
        {
            var response = _riotService.GetSummonerByName(name);
            return Ok(JsonConvert.SerializeObject(response));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
