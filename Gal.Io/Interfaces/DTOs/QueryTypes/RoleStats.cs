using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs.QueryTypes
{
    public class RoleStats
    {
        public string Role { get; set; }
        public int Matches { get; set; }
        public int TotalMatches { get; set; }
    }
}
