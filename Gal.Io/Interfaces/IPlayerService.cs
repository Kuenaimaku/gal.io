using Gal.Io.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gal.Io.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<PlayerDTO> FetchPlayers();
        PlayerDTO GetPlayer(Guid id);
        bool AddPlayer(PlayerDTO player);
        bool RemovePlayer(Guid id);
        PlayerDTO PatchPlayer(PlayerDTO player);
    }
}
