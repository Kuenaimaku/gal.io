using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gal.Io.Models
{
    [Table("ChampionBans")]
    public class ChampionBan
    {
        [Key]
        public Guid ChampionBanId { get; set; }

        [ForeignKey("Match"), Column("Match_Id", Order = 0)]
        public long MatchId { get; set; }
        public int ChampionKey { get; set; }
        [Required]
        public int PickTurn { get; set; }
        public int Side { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
