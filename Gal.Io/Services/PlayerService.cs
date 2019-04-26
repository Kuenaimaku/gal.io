using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gal.Io.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PlayerService> _logger;
        private readonly IRiotService _riotService;

        public PlayerService(IConfiguration config, ILogger<PlayerService> logger, IRiotService riotService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
        }

        public IEnumerable<PlayerDTO> FetchPlayers(string name)
        {
            try
            {
                List<PlayerDTO> response = new List<PlayerDTO>();
                using (var db = new DataContext())
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        foreach (var player in db.Players)
                        {
                            PlayerDTO p = new PlayerDTO()
                            {
                                PlayerId = player.PlayerId,
                                Name = player.Name,
                                SummonerName = player.SummonerName,
                                Notes = player.Notes,
                                SummonerDto = _riotService.GetSummonerByName(player.SummonerName)
                            };
                            response.Add(p);
                        }
                    }
                    else
                    {
                        IEnumerable<Player> players = db.Players.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase) || p.SummonerName.Contains(name, StringComparison.OrdinalIgnoreCase));
                        foreach(var player in players)
                        {
                            PlayerDTO p = new PlayerDTO()
                            {
                                PlayerId = player.PlayerId,
                                Name = player.Name,
                                SummonerName = player.SummonerName,
                                Notes = player.Notes,
                                SummonerDto = _riotService.GetSummonerByName(player.SummonerName)
                            };
                            response.Add(p);
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public PlayerDTO GetPlayer(Guid playerId)
        {
            try
            {
                PlayerDTO response = new PlayerDTO();
                using (var db = new DataContext())
                {
                    Player player = db.Players
                                        .Where(p => p.PlayerId == playerId)
                                        .FirstOrDefault();
                    if (player != null)
                    {
                        response.PlayerId = player.PlayerId;
                        response.Name = player.Name;
                        response.SummonerName = player.SummonerName;
                        response.Notes = player.Notes;
                        response.SummonerDto = _riotService.GetSummonerByName(player.SummonerName);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public bool AddPlayer(PlayerDTO player)
        {
            try
            {
                bool response = false;
                Player p = new Player()
                {
                    PlayerId = Guid.NewGuid(),
                    Name = player.Name,
                    SummonerName = player.SummonerName,
                    Notes = player.Notes
                };
                using (var db = new DataContext())
                {
                    db.Players.Add(p);
                    int count = db.SaveChanges();
                    _logger.LogInformation($"{count} records saved to database");
                    if (count == 1)
                        response = true;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public bool RemovePlayer(Guid playerId)
        {
            try
            {
                bool response = false;
                using (var db = new DataContext())
                {
                    Player player = db.Players
                                        .Where(p => p.PlayerId == playerId)
                                        .FirstOrDefault();
                    if (player != null)
                    {
                        db.Players.Remove(player);
                        int count = db.SaveChanges();
                        _logger.LogInformation($"{count} records removed from database");
                        if (count == 1)
                            response = true;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public PlayerDTO PatchPlayer(PlayerDTO player)
        {
            try
            { 
                var response = new PlayerDTO();
                using (var db = new DataContext())
                {
                    Player _p = db.Players
                                    .Where(p => p.PlayerId == player.PlayerId)
                                    .FirstOrDefault();
                    if (_p != null)
                    {
                        _p.Name = player.Name;
                        _p.SummonerName = player.SummonerName;
                        _p.Notes = player.Notes;
                        int count = db.SaveChanges();
                        _logger.LogInformation($"{count} records removed from database");
                        if (count == 1)
                            response = GetPlayer(_p.PlayerId);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }


}
