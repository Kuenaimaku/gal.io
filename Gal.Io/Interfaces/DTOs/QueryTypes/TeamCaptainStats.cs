using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs.QueryTypes
{
    public class TeamCaptainStats
    {
        public Guid PlayerId { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
    }
}
