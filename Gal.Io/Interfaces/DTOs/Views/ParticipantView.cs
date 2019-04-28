using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ParticipantView
    {


        public string Role { get; set; }

        //ParticipantStatDTO information
        public bool Win { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public long VisionScore { get; set; }

        public int GoldEarned { get; set; }
        public int GoldSpent { get; set; }
        public int TotalMinionsKilled { get; set; }

        public int Item0Id { get; set; }
        public int Item1Id { get; set; }
        public int Item2Id { get; set; }
        public int Item3Id { get; set; }
        public int Item4Id { get; set; }
        public int Item5Id { get; set; }
        public int Item6Id { get; set; }

        //ParticipantDTO information
        public SummonerSpellDTO SummonerSpell1 { get; set; }
        public SummonerSpellDTO SummonerSpell2 { get; set; }

        public virtual ChampionView Champion { get; set; }
        public virtual PlayerView Player { get; set; }

    }
}
