using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gal.Io.Services
{
    public class RiotService: IRiotService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RiotService> _logger;
        private readonly HttpClient _client;
        private readonly IChampionService _championService;

        public RiotService(IConfiguration config, ILogger<RiotService> logger, IChampionService championService)
        {
            _config = config.GetSection("Riot");
            _logger = logger;
            _championService = championService;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Riot-Token", _config.GetValue<string>("ApiKey"));
            _client.BaseAddress = _config.GetValue<Uri>("BaseUri");
        }

        public SummonerDTO GetSummonerByName(string name)
        {
            try
            {
                string endpoint = @"/lol/summoner/v4/summoners/by-name/";
                var response = _client.GetAsync($"{endpoint}{name}").Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var summoner = JsonConvert.DeserializeObject<SummonerDTO>(responseContent);
                return summoner;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public MatchDTO GetMatchByID(long MatchID){
            try{
                string endpoint = @"/lol/match/v4/matches/";
                var response = _client.GetAsync($"{endpoint}{MatchID}").Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var match = JsonConvert.DeserializeObject<MatchDTO>(responseContent);
                foreach( var participant in match.Participants)
                {
                    participant.Champion = _championService.GetChampionById(participant.ChampionId);
                }
                foreach(var team in match.Teams)
                {
                    team.Participants.AddRange(match.Participants.Where(x => x.TeamId == team.TeamId).ToList());
                    foreach (var ban in team.Bans)
                    {
                        ban.Champion = _championService.GetChampionById(ban.ChampionId);
                    }

                    //Order the Lists
                    team.Participants = team.Participants.OrderBy(x => x.ParticipantId).ToList();
                    team.Bans = team.Bans.OrderBy(x => x.PickTurn).ToList();
                }
                return match;
            }
            catch(Exception ex){
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }


}
