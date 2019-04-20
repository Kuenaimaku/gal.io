using Gal.Io.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("key")]
        public int Key { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("blurb")]
        public string Blurb { get; set; }
        [JsonProperty("image")]
        public ChampionImageDTO Image { get; set; }
    }
}
