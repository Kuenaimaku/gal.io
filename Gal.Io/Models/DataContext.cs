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

    }
}
