using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class CaptainBadge : BadgeView
    {
        public override string Title { get; set; } = "I'm the Captain Now";
        public override string Image1 { get; set; }
        public override string Image2 { get; set; } = null;
        public override string Icon { get; set; } = "bullhorn";
        public override string IconType { get; set; } = "is-info";
        public override string PlayerName { get; set; }
        public override string Description { get; set; } = "has lead their team to victory numerous times.";
        public override string Blurb { get; set; } = null;

    }
}
