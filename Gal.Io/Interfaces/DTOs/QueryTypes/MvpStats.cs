using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs.QueryTypes
{
    public class MvpStats
    {
        public Guid PlayerId { get; set; }
        public int MvpMatches { get; set; }
        public int RegularMatches { get; set; }
    }
}
