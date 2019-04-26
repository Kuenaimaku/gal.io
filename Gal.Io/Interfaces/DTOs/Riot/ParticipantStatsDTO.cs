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
        [JsonProperty("goldSpent")]
        public int GoldSpent { get; set; }
        [JsonProperty("totalMinionsKilled")]
        public int TotalMinionsKilled { get; set; }
        [JsonProperty("item0Id")]
        public int Item0Id { get; set; }
        [JsonProperty("item1Id")]
        public int Item1Id { get; set; }
        [JsonProperty("item2Id")]
        public int Item2Id { get; set; }
        [JsonProperty("item3Id")]
        public int Item3Id { get; set; }
        [JsonProperty("item4Id")]
        public int Item4Id { get; set; }
        [JsonProperty("item5Id")]
        public int Item5Id { get; set; }
        [JsonProperty("item6Id")]
        public int Item6Id { get; set; }
    }
}
