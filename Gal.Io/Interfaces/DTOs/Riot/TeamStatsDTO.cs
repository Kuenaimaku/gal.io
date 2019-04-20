using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class TeamStatsDTO
    {
        public TeamStatsDTO()
        {
            Participants = new List<ParticipantDTO>();
        }
        [JsonProperty("teamId")]
        public int TeamId { get; set; } // 100 = team 1, 200 = team 2
        [JsonProperty("win")]
        public string Win { get; set; }
        [JsonProperty("FirstBlood")]
        public bool FirstBlood { get; set; }
        [JsonProperty("firstDragon")]
        public bool FirstDragon { get; set; }
        [JsonProperty("firstRiftHerald")]
        public bool FirstRiftHerald { get; set; }
        [JsonProperty("firstBaron")]
        public bool FirstBaron { get; set; }
        [JsonProperty("firstTower")]
        public bool FirstTower { get; set; }
        [JsonProperty("firstInhibitor")]
        public bool FirstInhibitor { get; set; }
        [JsonProperty("dragonKills")]
        public int DragonKills { get; set; }
        [JsonProperty("baronKills")]
        public int BaronKills { get; set; }
        [JsonProperty("towerKills")]
        public int TowerKills { get; set; }
        [JsonProperty("inhibitorKills")]
        public int InhibitorKills { get; set; }
        [JsonProperty("vilemawKills")]
        public int VilemawKills { get; set; }
        [JsonProperty("bans")]
        public List<TeamBansDTO> Bans { get; set; }
        public List<ParticipantDTO> Participants { get; set; }


    }
}
