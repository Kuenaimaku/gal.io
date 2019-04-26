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
    public class MatchService : IMatchService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MatchService> _logger;
        private readonly IRiotService _riotService;
        private readonly IPlayerService _playerService;

        public MatchService(IConfiguration config, ILogger<MatchService> logger, IRiotService riotService, IPlayerService playerService)
        {
            _config = config;
            _logger = logger;
            _riotService = riotService;
            _playerService = playerService;
        }

        public IEnumerable<Match> GetMatches()
        {
            using (var db = new DataContext())
            {
                return db.Matches
                    .Include(x => x.ChampionPicks)
                    .Include(x => x.ChampionBans)
                    .Include(x => x.Participants)
                        .ThenInclude(y => y.Player)
                    .Include(x => x.PlayerStats)
                        .ThenInclude(y => y.Player)
                    .ToList();
            }
        }

        public bool CreateMatch(CreateMatchDTO request)
        {
            var response = false;
            using (var db = new DataContext())
            {
                //Check and see if the match exists
                var m = db.Matches.Where(x => x.MatchId == request.Match.GameId).FirstOrDefault();
                if (m == null)
                {
                    //If it does, do the various inserts inside of a transaction.
                    DateTime timestamp = DateTime.Now;
                    using (var Transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //Create the match.
                            var match = new Match
                            {
                                MatchId = request.Match.GameId,
                                UserId = request.User.UserId,
                                SeasonId = request.Match.SeasonId,
                                QueueId = request.Match.QueueId,
                                GameVersion = request.Match.GameVersion,
                                PlatformId = request.Match.PlatformId,
                                GameType = request.Match.GameType,
                                MapId = request.Match.MapId,
                                GameDuration = request.Match.GameDuration,
                                TimeStamp = timestamp
                            };
                            db.Matches.Add(match);
                            var matchCount = db.SaveChanges();
                            if(matchCount != 1)
                            {
                                throw new Exception("Match Insert Failed");
                            }

                            //Team 1 Inserts
                            for (var i = 0; i < request.Team1.Count(); i++)
                            {

                                var _player = request.Team1[i];
                                var _participant = request.Match.Participants[i];

                                //Insert Participant
                                var participant = new Participant
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    Side = 0,
                                    Order = i,
                                    Win = request.Match.Teams[0].Win == "Win",
                                    FirstBlood = request.Match.Teams[0].FirstBlood,
                                    FirstDragon = request.Match.Teams[0].FirstDragon,
                                    FirstRiftHerald = request.Match.Teams[0].FirstRiftHerald,
                                    FirstBaron = request.Match.Teams[0].FirstBaron,
                                    FirstTower = request.Match.Teams[0].FirstTower,
                                    FirstInhibitor = request.Match.Teams[0].FirstInhibitor,
                                    DragonKills = request.Match.Teams[0].DragonKills,
                                    BaronKills = request.Match.Teams[0].BaronKills,
                                    TowerKills = request.Match.Teams[0].TowerKills,
                                    InhibitorKills = request.Match.Teams[0].InhibitorKills,
                                    VilemawKills = request.Match.Teams[0].VilemawKills
                                };
                                db.Participants.Add(participant);
                                var participantCount = db.SaveChanges();
                                if (participantCount != 1)
                                {
                                    throw new Exception("Participant Insert Failed");
                                }

                                //Insert PlayerStats
                                var playerStats = new PlayerStats
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    ChampionKey = _participant.ChampionId,
                                    Role = _player.Role,
                                    Win = request.Match.Teams[0].Win == "Win",
                                    Kills = _participant.Stats.Kills,
                                    Deaths = _participant.Stats.Deaths,
                                    Assists = _participant.Stats.Assists,
                                    VisionScore = _participant.Stats.VisionScore,
                                    GoldEarned = _participant.Stats.GoldEarned,
                                    GoldSpent = _participant.Stats.GoldSpent,
                                    TotalMinionsKilled = _participant.Stats.TotalMinionsKilled,
                                    Item0Id = _participant.Stats.Item0Id,
                                    Item1Id = _participant.Stats.Item1Id,
                                    Item2Id = _participant.Stats.Item2Id,
                                    Item3Id = _participant.Stats.Item3Id,
                                    Item4Id = _participant.Stats.Item4Id,
                                    Item5Id = _participant.Stats.Item5Id,
                                    Item6Id = _participant.Stats.Item6Id,
                                    Spell1Id = _participant.Spell1Id,
                                    Spell2Id = _participant.Spell2Id,
                                    TimeStamp = timestamp
                                };

                                db.PlayerStats.Add(playerStats);
                                var playerStatsCount = db.SaveChanges();
                                if (playerStatsCount != 1)
                                {
                                    throw new Exception("PlayerStats Insert Failed");
                                }

                                //insert ChampionPick
                                var championPick = new ChampionPick
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    ChampionKey = _participant.ChampionId,
                                    TimeStamp = timestamp
                                };
                                db.ChampionPicks.Add(championPick);
                                var pickCount = db.SaveChanges();
                                if (pickCount != 1)
                                {
                                    throw new Exception("ChampionPick Insert Failed");
                                }
                            }

                            //Team 2 Inserts
                            for (var i = 0; i < request.Team2.Count(); i++)
                            {

                                var _player = request.Team2[i];
                                var _participant = request.Match.Participants[i+5];

                                //Insert Participant
                                var participant = new Participant
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    Side = 1,
                                    Order = i,
                                    Win = request.Match.Teams[1].Win == "Win",
                                    FirstBlood = request.Match.Teams[1].FirstBlood,
                                    FirstDragon = request.Match.Teams[1].FirstDragon,
                                    FirstRiftHerald = request.Match.Teams[1].FirstRiftHerald,
                                    FirstBaron = request.Match.Teams[1].FirstBaron,
                                    FirstTower = request.Match.Teams[1].FirstTower,
                                    FirstInhibitor = request.Match.Teams[1].FirstInhibitor,
                                    DragonKills = request.Match.Teams[1].DragonKills,
                                    BaronKills = request.Match.Teams[1].BaronKills,
                                    TowerKills = request.Match.Teams[1].TowerKills,
                                    InhibitorKills = request.Match.Teams[1].InhibitorKills,
                                    VilemawKills = request.Match.Teams[1].VilemawKills
                                };
                                db.Participants.Add(participant);
                                var participantCount = db.SaveChanges();
                                if (participantCount != 1)
                                {
                                    throw new Exception("Participant Insert Failed");
                                }

                                //Insert PlayerStats
                                var playerStats = new PlayerStats
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    ChampionKey = _participant.ChampionId,
                                    Role = _player.Role,
                                    Win = request.Match.Teams[1].Win == "Win",
                                    Kills = _participant.Stats.Kills,
                                    Deaths = _participant.Stats.Deaths,
                                    Assists = _participant.Stats.Assists,
                                    VisionScore = _participant.Stats.VisionScore,
                                    GoldEarned = _participant.Stats.GoldEarned,
                                    GoldSpent = _participant.Stats.GoldSpent,
                                    TotalMinionsKilled = _participant.Stats.TotalMinionsKilled,
                                    Item0Id = _participant.Stats.Item0Id,
                                    Item1Id = _participant.Stats.Item1Id,
                                    Item2Id = _participant.Stats.Item2Id,
                                    Item3Id = _participant.Stats.Item3Id,
                                    Item4Id = _participant.Stats.Item4Id,
                                    Item5Id = _participant.Stats.Item5Id,
                                    Item6Id = _participant.Stats.Item6Id,
                                    Spell1Id = _participant.Spell1Id,
                                    Spell2Id = _participant.Spell2Id,
                                    TimeStamp = timestamp
                                };

                                db.PlayerStats.Add(playerStats);
                                var playerStatsCount = db.SaveChanges();
                                if (playerStatsCount != 1)
                                {
                                    throw new Exception("PlayerStats Insert Failed");
                                }

                                //insert ChampionPick
                                var championPick = new ChampionPick
                                {
                                    MatchId = request.Match.GameId,
                                    PlayerId = _player.PlayerId,
                                    ChampionKey = _participant.ChampionId,
                                    TimeStamp = timestamp
                                };
                                db.ChampionPicks.Add(championPick);
                                var pickCount = db.SaveChanges();
                                if (pickCount != 1)
                                {
                                    throw new Exception("ChampionPick Insert Failed");
                                }
                            }

                            //Add Bans
                            foreach (var team in request.Match.Teams)
                            {
                                foreach(var ban in team.Bans)
                                {
                                    var championBan = new ChampionBan
                                    {
                                        MatchId = request.Match.GameId,
                                        ChampionKey = ban.ChampionId,
                                        PickTurn = ban.PickTurn,
                                        TimeStamp = timestamp
                                    };
                                    db.ChampionBans.Add(championBan);
                                    var banCount = db.SaveChanges();
                                    if (banCount != 1)
                                    {
                                        throw new Exception("ChampionBan Insert Failed");
                                    }
                                }
                            }

                            Transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            Transaction.Rollback();
                        }
                    }
                    response = true;
                }
            }
            return response;
        }
    }
}
