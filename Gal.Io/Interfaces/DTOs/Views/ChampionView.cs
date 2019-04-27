using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class ChampionView
    {
        public string Id { get; set; }
        public int Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public string Image { get; set; }
    }
}
