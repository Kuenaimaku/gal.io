using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gal.Io.Interfaces.DTOs.QueryTypes;
using Microsoft.EntityFrameworkCore;

namespace Gal.Io.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ChampionPick> ChampionPicks { get; set; }
        public DbSet<ChampionBan> ChampionBans { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./data.db");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Query<BestAlly>();
            modelBuilder.Query<TeamCaptainStats>();
            modelBuilder.Query<MvpStats>();
            modelBuilder.Query<RivalStats>();
            modelBuilder.Query<RoleStats>();
        }

        public async Task<List<BestAlly>> GetBestAlly(Guid playerId)
        {
            var bestAlly = await this.Query<BestAlly>().FromSql($@"SELECT p.PlayerId, COUNT(DISTINCT m.MatchId) as Wins FROM [Matches] m 
                                        JOIN Participants par ON m.MatchID = par.Match_Id
                                        JOIN Players p ON par.Player_Id = p.PlayerId
                                        JOIN (
                                        SELECT Match_Id, Side FROM Participants p WHERE Player_ID IN (
                                        SELECT PlayerID FROM Players WHERE PlayerId = {playerId}
                                        )
                                        AND p.Win = true
                                        ) mpar ON m.MatchID = mpar.Match_Id AND par.Side = mpar.Side
                                        WHERE p.PlayerId <> {playerId}
                                        GROUP BY p.Name
                                        ORDER BY COUNT(DISTINCT m.MatchId) DESC 
                                        LIMIT 1;").ToListAsync();

            return bestAlly;

        }

        public async Task<List<TeamCaptainStats>> GetTeamCaptainStats(Guid playerId)
        {
            var res = await this.Query<TeamCaptainStats>().FromSql($@"SELECT Player_Id as PlayerId, 
                                        COUNT(Match_Id) as Matches, 
                                        SUM(CASE WHEN par.Win = 1 THEN 1 ELSE 0 end) as Wins 
                                        FROM Participants par
                                            JOIN Players p ON par.Player_Id = p.PlayerId
                                        WHERE [Order] = 0
                                        AND p.PlayerId = {playerId}
                                        GROUP BY Player_Id
                                        ORDER BY COUNT(Match_Id) DESC, p.Name").ToListAsync();

            return res;

        }

        public async Task<List<MvpStats>> GetMvpStats(Guid playerId)
        {
            var res = await this.Query<MvpStats>().FromSql($@"SELECT PlayerId, mvp.Matches as MvpMatches, reg.Matches as RegularMatches
                                        FROM Players p 
                                        JOIN (
                                            SELECT Player_Id, COUNT(Match_Id) as Matches
                                            FROM Participants par
                                            WHERE [Order] IN (1,2)
                                            GROUP BY Player_Id
                                        ) mvp on p.PlayerId = mvp.Player_Id

                                        JOIN (
                                            SELECT Player_Id, COUNT(Match_Id) as Matches
                                            FROM Participants par
                                            WHERE [Order] IN (0,3,4)
                                            GROUP BY Player_Id
                                        ) reg on p.PlayerId = reg.Player_Id
                                        WHERE PlayerId = {playerId}").ToListAsync();
            return res;
        }

        public async Task<List<RivalStats>> GetRivalStats(Guid playerId)
        {
            var res = await this.Query<RivalStats>().FromSql($@"SELECT par.Player_Id as PlayerId, COUNT(par.Match_Id) as Matches FROM Participants par
                                        JOIN (
	                                        SELECT Match_Id, Side FROM Participants p WHERE Player_ID IN (
		                                        SELECT PlayerID FROM Players WHERE PlayerId = {playerId}
	                                        )
                                        ) mpar WHERE par.Match_Id = mpar.Match_Id AND par.Side <> mpar.Side
                                        GROUP BY par.Player_Id
                                        ORDER BY COUNT(par.Match_Id) DESC").ToListAsync();
            return res;
        }

        public async Task<List<RoleStats>> GetRoleStats(Guid playerId)
        {
            var res = await this.Query<RoleStats>().FromSql($@"SELECT ro.Role, ro.Matches, COUNT(DISTINCT Match_Id) as TotalMatches
                                        FROM Participants p
                                        JOIN(
	                                        SELECT ps.Player_Id, ps.Role, COUNT(p.Match_Id) as Matches FROM Participants p
	                                        JOIN PlayerStats ps ON p.Player_Id = ps.Player_Id AND p.Match_Id = ps.Match_Id
	                                        WHERE p.Player_ID IN (
		                                        SELECT PlayerID FROM Players WHERE PlayerId = {playerId}
	                                        )
	                                        GROUP BY ps.Player_Id, ps.Role
                                        ) ro ON p.Player_Id = ro.Player_Id").ToListAsync();
            return res;
        }
    }
}
