using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionFilterDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
