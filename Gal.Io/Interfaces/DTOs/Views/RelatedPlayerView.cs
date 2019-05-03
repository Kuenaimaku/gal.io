using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class RelatedPlayerView
    {

        public RelatedPlayerView()
        {
            Data = new Dictionary<string, string>();
        }

        public PlayerView Player;

        public Dictionary<string, string> Data { get; set; }
    }
}
