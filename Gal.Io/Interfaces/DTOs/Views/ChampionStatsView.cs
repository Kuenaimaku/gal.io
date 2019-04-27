using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionStatsView
    {
        public ChampionView Champion { get; set; }
        public int TotalMatches { get; set; }
        public int Picks { get; set; }
        public int Wins { get; set; }
        public int BlueBans { get; set; }
        public int RedBans { get; set; }
    }
}
