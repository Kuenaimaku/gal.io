using AutoMapper;
using Gal.Io.Models;
using Gal.Io.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Extensions
{
    public class DomainProfile: Profile
    {
        public DomainProfile()
        {
            CreateMap<Match, MatchView>();
            CreateMap<ChampionDTO, ChampionView>();
            CreateMap<Participant, ParticipantView>();
            CreateMap<ChampionBan, ChampionBanView>();
            CreateMap<Player, PlayerView>();
            CreateMap<Player, PlayerDetailView>();
        }
    }
}
