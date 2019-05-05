using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{ 
    public class BadgeView
    {
        public virtual string Title { get; set; }
        public virtual string Image1 { get; set; }
        public virtual string Image2 { get; set; }
        public virtual string Icon { get; set; }
        public virtual string IconType { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Blurb { get; set; }
    }

    public class RelationalBadgeView
    {
        public virtual string Title { get; set; }
        public virtual string Image1 { get; set; }
        public virtual string Image2 { get; set; }
        public virtual string Icon { get; set; }
        public virtual string IconType { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual string RelatedPlayerName { get; set; }
        public virtual Guid RelatedPlayerId { get; set; }
        public virtual string Relationship { get; set; }
        public virtual string Blurb1 { get; set; }
        public virtual string Blurb2 { get; set; }
    }
}
