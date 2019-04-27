using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gal.Io.Models
{
    [Table("ChampionPicks")]
    public class ChampionPick
    {

        [Key]
        public Guid ChampionPickId { get; set; }
        [ForeignKey("Match"), Column("Match_Id", Order = 0)]
        public long MatchId { get; set; }

        //Player
        [ForeignKey("Player"), Column("Player_Id", Order = 1)]
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        //Misc
        [Required, Column("Champion_Key", Order = 2)]
        public int ChampionKey { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
