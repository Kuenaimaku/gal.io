using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Models
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(16), MinLength(3), Required]
        public string SummonerName { get; set; }
        [MaxLength(100)]
        public string Notes { get; set; }

        public virtual List<ChampionPick> ChampionPicks { get; set; }
        public virtual List<PlayerStats> PlayerStats { get; set; }
        public virtual List<Participant> Participations { get; set; }


    }
}
