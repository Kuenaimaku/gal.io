using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoMapper;
using Gal.Io.Models;

namespace Gal.Io.Services
{
    public class ChampionService : IChampionService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ChampionService> _logger;
        private readonly IMapper _mapper;
        private readonly IRiotService _riotService;

        public ChampionService(IConfiguration config, ILogger<ChampionService> logger, IMapper mapper, IRiotService riotService)
        {
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _riotService = riotService;
            
        }

        public IEnumerable<ChampionStatsView> FetchChampions()
        {
            List<ChampionStatsView> response = new List<ChampionStatsView>();
            var champions = _riotService.FetchChampions();
            using (var db = new DataContext())
            {
                foreach (var champion in champions)
                {
                    var _c = _mapper.Map<ChampionView>(champion);
                    _c.Image = champion.Image.Full;
                    int picks = db.ChampionPicks.Where(x => x.ChampionKey == _c.Key).Count();
                    int blueBans = db.ChampionBans.Where(x => x.ChampionKey == _c.Key && x.Side == 0).Count();
                    int redBans = db.ChampionBans.Where(x => x.ChampionKey == _c.Key && x.Side == 1).Count();
                    int wins = (from cp in db.ChampionPicks
                                join par in db.Participants
                                on new { cp.MatchId, cp.PlayerId } equals new { par.MatchId, par.PlayerId }
                                where par.Win == true && cp.ChampionKey == _c.Key
                                select par.ParticipantId).Count();
                    int totalMatches = db.Matches.Count();
                    var championStats = new ChampionStatsView
                    {
                        Champion = _c,
                        TotalMatches = totalMatches,
                        Picks = picks,
                        Wins = wins,
                        BlueBans = blueBans,
                        RedBans = redBans
                    };
                    response.Add(championStats);
                }
            }
            return response;
        }
    }
}