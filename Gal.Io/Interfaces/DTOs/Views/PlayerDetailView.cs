using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class PlayerDetailView
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public string SummonerName { get; set; }
        public string Notes { get; set; }
        public SummonerDTO LeagueAccount { get; set; }

        public RelatedPlayerView BestAlly { get; set; }
    }
}
