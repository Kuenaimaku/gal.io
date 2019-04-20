using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class MatchDTO
    {
        [JsonProperty("seasonId")]
        public int SeasonId { get; set; }
        [JsonProperty("queueId")]
        public int QueueId { get; set; }
        [JsonProperty("gameId")]
        public long GameId { get; set; }
        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }
        [JsonProperty("platformId")]
        public string PlatformId { get; set; }
        [JsonProperty("gameType")]
        public string GameType { get; set; }
        [JsonProperty("mapId")]
        public string MapId { get; set; }
        [JsonProperty("gameDuration")]
        public long GameDuration { get; set; }
        [JsonProperty("teams")]
        public List<TeamStatsDTO> Teams { get; set; }
        [JsonProperty("Participants")]
        public List<ParticipantDTO> Participants { get; set; }
    }
}
