using League_Recorder_Backend.Interfaces;
using League_Recorder_Backend.Interfaces.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace League_Recorder_Backend.Services
{
    public class RiotService: IRiotService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RiotService> _logger;
        private readonly HttpClient _client;

        public RiotService(IConfiguration config, ILogger<RiotService> logger)
        {
            _config = config.GetSection("Riot");
            _logger = logger;
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
        

    }


}
