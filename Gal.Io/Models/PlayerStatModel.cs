using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Models
{
    [Table("PlayerStats")]
    public class PlayerStats
    {
        //The Match this TeamMember is for
        [ForeignKey("Match"), Column("Match_Id", Order = 0)]
        public long MatchId { get; set; }
        public virtual Match Match { get; set; }

        //The player this TeamMember is for
        [ForeignKey("Player"), Column("Player_Id", Order = 1)]
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        //The championKey this relates to
        [Required, Column("Champion_Key", Order = 2)]
        public int ChampionKey { get; set; }
        [Required]
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
        public int Spell1Id { get; set; }
        public int Spell2Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }


    }
}
