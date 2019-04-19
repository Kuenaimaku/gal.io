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

namespace Gal.Io.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<PlayersController> _logger;
        private readonly IRiotService _riotService;
        private readonly IPlayerService _playerService;
        public PlayersController(IConfiguration config, ILogger<PlayersController> logger, IRiotService riotService, IPlayerService playerService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
            _playerService = playerService;
        }

        // GET api/players
        [HttpGet]
        public ActionResult<IEnumerable<PlayerDTO>> Get()
        {
            var response = _playerService.FetchPlayers();
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet("{name}")]
        public ActionResult <SummonerDTO> Validate(string name)
        {
            var response = _riotService.GetSummonerByName(name);
            return Ok(JsonConvert.SerializeObject(response));
        }

        // PATCH api/players
        [HttpPatch]
        public PlayerDTO Patch([FromBody] PlayerDTO player)
        {
            var response = _playerService.PatchPlayer(player);
            return response;
        }

        // PUT api/players
        [HttpPut]
        public ActionResult Put([FromBody] PlayerDTO player)
        {
            var r = _playerService.AddPlayer(player);
            if (r)
                return Ok();
            else
                return BadRequest();

        }

        // DELETE api/players/guid
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var r = _playerService.RemovePlayer(id);
            if (r)
                return Ok();
            else
                return BadRequest();
        }
    }
}
