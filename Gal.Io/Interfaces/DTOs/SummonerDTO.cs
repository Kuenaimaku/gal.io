using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class SummonerDTO
    {
        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("summonerLevel")]
        public long SummonerLevel { get; set; }
        [JsonProperty("revisionDate")]
        public long RevisionDate { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("accountId")]
        public string AccountId { get; set; }
        [JsonProperty("puuid")]
        public string Puuid { get; set; }
    }
}
