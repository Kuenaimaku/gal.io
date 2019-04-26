using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Models
{
    [Table("Matches")]
    public class Match
    {
        [Key]
        public long MatchId { get; set; }

        //Whoever Logged the Match
        [ForeignKey("User"), Column("User_Id")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
      

        //Match Details

        public int SeasonId { get; set; }
        public int QueueId { get; set; }
        public string GameVersion { get; set; }
        public string PlatformId { get; set; }
        public string GameType { get; set; }
        public string MapId { get; set; }
        public long GameDuration { get; set; }

        public DateTime TimeStamp { get; set; }

        //Intersections
        public virtual List<ChampionBan> ChampionBans { get; set; }
        public virtual List<ChampionPick> ChampionPicks { get; set; }
        public virtual List<Participant> Participants { get; set; }
        public virtual List<PlayerStats> PlayerStats { get; set; }
    }
}
