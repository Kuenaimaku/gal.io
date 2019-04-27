using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionBanView
    {
        public int PickTurn { get; set; }
        public virtual ChampionView Champion { get; set; }
    }
}
