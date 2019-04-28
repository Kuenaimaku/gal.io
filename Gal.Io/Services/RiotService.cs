using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly IList<ChampionDTO> _champions;
        private readonly IList<SummonerSpellDTO> _summonerSpells;

        public RiotService(IConfiguration config, ILogger<RiotService> logger)
        {
            _config = config.GetSection("Riot");
            _logger = logger;
            _champions = new List<ChampionDTO>();
            _summonerSpells = new List<SummonerSpellDTO>();
            BuildChampionCache();
            BuildSummonerCache();
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Riot-Token", _config.GetValue<string>("ApiKey"));
            _client.BaseAddress = _config.GetValue<Uri>("BaseUri");
        }

        private void BuildChampionCache()
        {
            var _client = new HttpClient();
            var url = @"http://ddragon.leagueoflegends.com/cdn/9.8.1/data/en_US/champion.json";
            var response = _client.GetAsync($"{url}").Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            JObject championJson = JObject.Parse(responseContent)["data"].Value<JObject>();
            foreach (var champion in championJson)
            {
                _champions.Add(JsonConvert.DeserializeObject<ChampionDTO>(champion.Value.ToString()));
            }

        }

        private void BuildSummonerCache()
        {
            var _client = new HttpClient();
            var url = @"http://ddragon.leagueoflegends.com/cdn/9.8.1/data/en_US/summoner.json";
            var response = _client.GetAsync($"{url}").Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            JObject summonerSpellJson = JObject.Parse(responseContent)["data"].Value<JObject>();
            foreach (var summonerSpell in summonerSpellJson)
            {
                _summonerSpells.Add(JsonConvert.DeserializeObject<SummonerSpellDTO>(summonerSpell.Value.ToString()));
            }
        }

        public ChampionDTO GetChampionById(int id)
        {
            return _champions.Where(x => x.Key == id).FirstOrDefault();

        }

        public IEnumerable<ChampionDTO> FetchChampions()
        {
            return _champions.OrderBy(x => x.Name);
        }

        public SummonerSpellDTO GetSummonerSpellById(int id)
        {
            return _summonerSpells.Where(x => x.Key == id).FirstOrDefault();

        }

        public IEnumerable<SummonerSpellDTO> FetchSummonerSpells()
        {
            return _summonerSpells.OrderBy(x => x.Name);
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
                    participant.Champion = GetChampionById(participant.ChampionId);
                }
                foreach(var team in match.Teams)
                {
                    team.Participants.AddRange(match.Participants.Where(x => x.TeamId == team.TeamId).ToList());
                    foreach (var ban in team.Bans)
                    {
                        ban.Champion = GetChampionById(ban.ChampionId);
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
