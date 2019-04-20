using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class TeamBansDTO
    {
        [JsonProperty("pickTurn")]
        public int PickTurn { get; set; }
        [JsonProperty("championId")]
        public int ChampionId { get; set; }
        public ChampionDTO Champion { get; set; }
        


    }
}
