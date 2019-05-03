using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMatchService _matchService;
        public MatchesController(IConfiguration config, ILogger<MatchesController> logger, IRiotService riotService, IMatchService matchService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
            _matchService = matchService;
        }

        // GET api/players
        [HttpGet]
        public IEnumerable<MatchView> Get()
        {
            var res = _matchService.GetMatches();
            return res;
        }

        // GET api/matches/by-player/id
        [HttpGet("by-player/{id}")]
        public IEnumerable<MatchView> GetMatchesByPlayerId(Guid id)
        {
            var res = _matchService.GetMatchesFiltered(new MatchFilterDTO { PlayerId = id });
            return res;
        }

        [HttpGet("{MatchID}")]
        public ActionResult<MatchDTO> Validate(long MatchID)
        {
            var response = _riotService.GetMatchByID(MatchID);
            return Ok(JsonConvert.SerializeObject(response));
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] CreateMatchDTO match)
        {
            bool response = _matchService.CreateMatch(match);
            if (response)
                return Ok();
            else
                return BadRequest();
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
