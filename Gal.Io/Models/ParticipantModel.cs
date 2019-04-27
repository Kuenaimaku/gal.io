using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Models
{
    [Table("Participants")]
    public class Participant
    {
        [Key]
        public Guid ParticipantId { get; set; }
        //The Match this TeamMember is for
        [ForeignKey("Match"), Column("Match_Id", Order = 0)]
        public long MatchId { get; set; }
        public virtual Match Match { get; set; }

        //The player this TeamMember is for
        [ForeignKey("Player"), Column("Player_Id", Order = 1)]
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        //TeamMember-Specific information that isn't performance related
        [Required]
        public int Side { get; set; }
        [Required]
        public int Order { get; set; }

        //Team-Specific information
        public bool Win { get; set; }
        public bool FirstBlood { get; set; }
        public bool FirstDragon { get; set; }
        public bool FirstRiftHerald { get; set; }
        public bool FirstBaron { get; set; }
        public bool FirstTower { get; set; }
        public bool FirstInhibitor { get; set; }
        public int DragonKills { get; set; }
        public int BaronKills { get; set; }
        public int TowerKills { get; set; }
        public int InhibitorKills { get; set; }
        public int VilemawKills { get; set; }
        public DateTime TimeStamp { get; set; }

        //Intersections

    }
}
