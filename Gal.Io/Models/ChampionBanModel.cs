using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gal.Io.Models
{
    [Table("ChampionBans")]
    public class ChampionBan
    {
        [Key, Column("Match_Id", Order = 0)]
        public long MatchId { get; set; }
        [Key, Column("Champion_Key", Order = 1)]
        public int ChampionKey { get; set; }
        [Required]
        public int PickTurn { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
