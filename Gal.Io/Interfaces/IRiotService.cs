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
        String GetMatchByID(long MatchID);
    }
}
