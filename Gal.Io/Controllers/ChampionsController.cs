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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gal.Io.Controllers
{
    [Route("api/[controller]")]
    public class ChampionsController : Controller
    {

        private readonly IConfiguration _config;
        private readonly ILogger<ChampionsController> _logger;
        private readonly IRiotService _riotService;
        private readonly IChampionService _championService;
        public ChampionsController(IConfiguration config, ILogger<ChampionsController> logger, IRiotService riotService, IChampionService championService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
            _championService = championService;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<ChampionStatsView> Get()
        {
            return _championService.FetchChampions();
        }
    }
}
