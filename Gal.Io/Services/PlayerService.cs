using AutoMapper;
using Gal.Io.Interfaces;
using Gal.Io.Interfaces.DTOs;
using Gal.Io.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;

        public PlayerService(IConfiguration config, ILogger<PlayerService> logger, IMapper mapper, IRiotService riotService)
        {
            _config = config;
            _logger = logger;
            _mapper = mapper;
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

        public PlayerDetailView GetPlayerDetailed(Guid playerId)
        {
            try
            {
                PlayerDetailView response = new PlayerDetailView();
                using (var db = new DataContext())
                {
                    Player player = db.Players
                                        .Where(p => p.PlayerId == playerId)
                                        .FirstOrDefault();
                    if (player != null)
                    {
                        response = _mapper.Map<PlayerDetailView>(player);
                        response.LeagueAccount = _riotService.GetSummonerByName(player.SummonerName);


                        var r = db.GetBestAlly(playerId).Result.ToList().First();
                        var ba = new RelatedPlayerView();
                        ba.Player = _mapper.Map<PlayerView>(db.Players.Where(x=>x.PlayerId == r.PlayerId).First());
                        ba.Data["Wins"] = r.Wins.ToString();
                        response.BestAlly = ba;
                        response.BestAlly.Player.LeagueAccount = _riotService.GetSummonerByName(response.BestAlly.Player.SummonerName);

                        var rival = db.GetRivalStats(playerId).Result.ToList().First();
                        var ri = new RelatedPlayerView();
                        ri.Player = _mapper.Map<PlayerView>(db.Players.Where(x => x.PlayerId == rival.PlayerId).First());
                        ri.Data["Wins"] = rival.Matches.ToString();
                        response.Rival = ri;
                        response.Rival.Player.LeagueAccount = _riotService.GetSummonerByName(response.Rival.Player.SummonerName);

                        var _tcs = db.GetTeamCaptainStats(playerId).Result.ToList().FirstOrDefault();
                        if (_tcs != null && (_tcs.Wins * 100.0 / _tcs.Matches) >= 50.0)
                            response.PlayerBadges.Add("Captain");

                        var _mvp = db.GetMvpStats(playerId).Result.ToList().FirstOrDefault();
                        if (_mvp != null && (_mvp.MvpMatches * 100.0 / _mvp.RegularMatches) >= 50.0)
                            response.PlayerBadges.Add("MVP");

                        var _role = db.GetRoleStats(playerId).Result.ToList().FirstOrDefault();
                        if (_role != null && (_role.Matches * 100.0 / _role.TotalMatches) >= 50.0)
                            response.PlayerBadges.Add($"Role-{_role.Role}");



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
