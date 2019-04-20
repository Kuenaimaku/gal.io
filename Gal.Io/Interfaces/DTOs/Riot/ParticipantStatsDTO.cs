using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ParticipantStatsDTO
    {
        [JsonProperty("kills")]
        public int Kills { get; set; }
        [JsonProperty("deaths")]
        public int Deaths { get; set; }
        [JsonProperty("assists")]
        public int Assists { get; set; }
        [JsonProperty("visionScore")]
        public long VisionScore { get; set; }
        [JsonProperty("win")]
        public bool Win { get; set; }
        [JsonProperty("goldEarned")]
        public int GoldEarned { get; set; }
        [JsonProperty("totalMinionsKilled")]
        public int TotalMinionsKilled { get; set; }
    }
}
