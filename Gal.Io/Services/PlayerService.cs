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

                        //Get the related player badges
                        var ally = db.GetBestAlly(playerId).Result.ToList().First();
                        var allyPlayer = GetPlayer(ally.PlayerId);
                        var allySummoner = _riotService.GetSummonerByName(allyPlayer.SummonerName);
                        var allyBadge = new RelationalBadgeView
                        {
                            Title = "Friends Forever",
                            Image1 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{response.LeagueAccount.ProfileIconId}.png",
                            Image2 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{allySummoner.ProfileIconId}.png",
                            Icon = "heart",
                            IconType = "is-danger",
                            PlayerName = player.SummonerName,
                            RelatedPlayerName = allyPlayer.SummonerName,
                            RelatedPlayerId = allyPlayer.PlayerId,
                            Relationship = "the Best of Allies",
                            Blurb1 = "wins the most when",
                            Blurb2 = "is on their team."
                        };
                        response.RelationalBadges.Add(allyBadge);
                        var rival = db.GetRivalStats(playerId).Result.ToList().First();
                        var rivalPlayer = GetPlayer(rival.PlayerId);
                        var rivalSummoner = _riotService.GetSummonerByName(rivalPlayer.SummonerName);
                        var rivalBadge = new RelationalBadgeView
                        {
                            Title = "It doesn't have to be this way...",
                            Image1 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{response.LeagueAccount.ProfileIconId}.png",
                            Image2 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{rivalSummoner.ProfileIconId}.png",
                            Icon = "flash",
                            IconType = "is-warning",
                            PlayerName = player.SummonerName,
                            RelatedPlayerName = rivalPlayer.SummonerName,
                            RelatedPlayerId = rivalPlayer.PlayerId,
                            Relationship = "Fated Rivals",
                            Blurb1 = "always seems to be against",
                            Blurb2 = ""
                        };
                        response.RelationalBadges.Add(rivalBadge);


                        //Get the stats required to see if a generic badge is to be rewarded
                        var _tcs = db.GetTeamCaptainStats(playerId).Result.ToList().FirstOrDefault();
                        if (_tcs != null && (_tcs.Wins * 100.0 / _tcs.Matches) >= 50.0)
                        {
                            var captain = new CaptainBadge
                            {
                                PlayerName = player.SummonerName,
                                Image1 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{response.LeagueAccount.ProfileIconId}.png",
                            };
                            response.GenericBadges.Add(captain);
                        }

                        var _mvp = db.GetMvpStats(playerId).Result.ToList().FirstOrDefault();
                        if (_mvp != null && (_mvp.MvpMatches * 100.0 / _mvp.RegularMatches) >= 50.0)
                        {
                            var mvp = new MVPBadge
                            {
                                PlayerName = player.SummonerName,
                                Image1 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{response.LeagueAccount.ProfileIconId}.png",
                            };
                            response.GenericBadges.Add(mvp);
                            
                        }
                        var _role = db.GetRoleStats(playerId).Result.ToList().FirstOrDefault();
                        if (_role != null && (_role.Matches * 100.0 / _role.TotalMatches) >= 50.0)
                        {
                            var role = new RoleBadge
                            {
                                PlayerName = player.SummonerName,
                                Image1 = $@"http://ddragon.leagueoflegends.com/cdn/9.8.1/img/profileicon/{response.LeagueAccount.ProfileIconId}.png",
                            };
                            role.Title += _role.Role;
                            role.Image2 = $@"/assets/role-{_role.Role.ToLower()}.png";
                            response.GenericBadges.Add(role);
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
