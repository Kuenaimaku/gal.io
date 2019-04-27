using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces.DTOs
{
    public class TeamView
    {
        public TeamView()
        {
            Bans = new List<ChampionBanView>();
            Participants = new List<ParticipantView>();
        }
        public bool Win { get; set; }
        public bool FirstBlood { get; set; }
        public bool FirstDragon { get; set; }
        public bool FirstRiftHerald { get; set; }
        public bool FirstBaron { get; set; }
        public bool FirstTower { get; set; }
        public bool FirstInhibitor { get; set; }
        public int DragonKills { get; set; }
        public int BaronKills { get; set; }
        public int TowerKills { get; set; }
        public int InhibitorKills { get; set; }
        public int VilemawKills { get; set; }

        public virtual List<ChampionBanView> Bans { get; set; }
        public virtual List<ParticipantView> Participants { get; set; }


    }
}
