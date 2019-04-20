using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ParticipantDTO
    {
        [JsonProperty("participantId")]
        public int ParticipantId { get; set; }
        [JsonProperty("teamId")]
        public int TeamId { get; set; }
        [JsonProperty("spell1Id")]
        public int Spell1Id { get; set; }
        [JsonProperty("spell2Id")]
        public int Spell2Id { get; set; }
        [JsonProperty("championId")]
        public int ChampionId { get; set; }
        [JsonProperty("stats")]
        public ParticipantStatsDTO Stats { get; set; }
        public ChampionDTO Champion { get; set; }

    }
}
