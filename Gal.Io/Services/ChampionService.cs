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

namespace Gal.Io.Services
{
    public class ChampionService : IChampionService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ChampionService> _logger;
        private readonly IList<ChampionDTO> _champions;

        public ChampionService(IConfiguration config, ILogger<ChampionService> logger)
        {
            _config = config;
            _logger = logger;
            _champions = new List<ChampionDTO>();
            BuildChampionCache();
            
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

        public ChampionDTO GetChampionById(int id)
        {
            return _champions.Where(x => x.Key == id).FirstOrDefault();
            
        }
    }
}