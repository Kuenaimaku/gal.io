using Gal.Io.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces
{
    public interface IRiotService
    {
        SummonerDTO GetSummonerByName(string name);
        MatchDTO GetMatchByID(long MatchID);

        ChampionDTO GetChampionById(int id);

        IEnumerable<ChampionDTO> FetchChampions();
    }
}
