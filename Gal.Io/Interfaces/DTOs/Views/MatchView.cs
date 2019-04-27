using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class MatchView
    {

        public MatchView()
        {
            Teams = new List<TeamView>();
        }

        public long MatchId { get; set; }
        public virtual UserView User { get; set; }


        //Match Details

        public int SeasonId { get; set; }
        public int QueueId { get; set; }
        public string GameVersion { get; set; }
        public string PlatformId { get; set; }
        public string GameType { get; set; }
        public string MapId { get; set; }
        public long GameDuration { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<TeamView> Teams { get; set; }
    }
}
