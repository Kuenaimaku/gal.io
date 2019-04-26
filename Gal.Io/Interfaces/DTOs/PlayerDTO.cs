using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class PlayerDTO
    {
        [JsonProperty("id")]
        public Guid PlayerId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; } 
        [JsonProperty("summonerDto")]
        public SummonerDTO SummonerDto { get; set; }
    }
}
