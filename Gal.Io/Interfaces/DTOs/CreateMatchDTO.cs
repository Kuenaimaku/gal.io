using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gal.Io.Interfaces.DTOs;
using Newtonsoft.Json;

namespace Gal.Io.Interfaces.DTOs
{
    public class CreateMatchDTO
    {
        public MatchDTO Match { get; set; }

        public List<CreateMatchPlayerDTO> Team1 { get; set; }
        public List<CreateMatchPlayerDTO> Team2 { get; set; }
        public UserDTO User { get; set; }

    }

    public class CreateMatchPlayerDTO
    {
        [JsonProperty("id")]
        public Guid PlayerId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
