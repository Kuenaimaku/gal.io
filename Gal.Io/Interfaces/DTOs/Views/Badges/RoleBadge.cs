using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class RoleBadge : BadgeView
    {
        public override string Title { get; set; } = "Role Player - ";
        public override string Image1 { get; set; }
        public override string Image2 { get; set; } = null;
        public override string Icon { get; set; } = "heart";
        public override string IconType { get; set; } = "is-danger";
        public override string PlayerName { get; set; }
        public override string Description { get; set; } = "tends to get this role a majority of the time.";
        public override string Blurb { get; set; } = null;

    }
}
