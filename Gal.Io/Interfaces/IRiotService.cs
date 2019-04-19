using League_Recorder_Backend.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace League_Recorder_Backend.Interfaces
{
    public interface IRiotService
    {
        SummonerDTO GetSummonerByName(string name);
    }
}
