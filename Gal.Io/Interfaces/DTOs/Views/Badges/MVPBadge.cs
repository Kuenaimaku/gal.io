using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class MVPBadge: BadgeView
    {
        public override string Title { get; set; } = "Hate Me Cuz They Ain't Me";
        public override string Image1 { get; set; }
        public override string Image2 { get; set; } = null;
        public override string Icon { get; set; } = "star-circle";
        public override string IconType { get; set; } = "is-info";
        public override string PlayerName { get; set; }
        public override string Description { get; set; } = "is high on the priority list for captains.";
        public override string Blurb { get; set; } = "Don't hate the player, hate the game";

    }
}
