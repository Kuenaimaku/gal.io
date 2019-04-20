using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionImageDTO
    {
        [JsonProperty("full")]
        public string Full { get; set; }

    }
}
