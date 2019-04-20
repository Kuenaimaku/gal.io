using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace League_Recorder_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<MatchesController> _logger;
        private readonly IRiotService _riotService;
        public MatchesController(IConfiguration config, ILogger<MatchesController> logger, IRiotService riotService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
        }

        // GET api/players
        [HttpGet]
        public void Get()
        {
        }

        [HttpGet("{MatchID}")]
        public ActionResult <String> Validate(long MatchID)
        {
            var response = _riotService.GetMatchByID(MatchID);
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
