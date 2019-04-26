using System;
using System.Collections.Generic;
using System.IO;
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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChampionBan>()
                .HasKey(c => new { c.MatchId, c.ChampionKey });

            modelBuilder.Entity<ChampionPick>()
                .HasKey(c => new { c.MatchId, c.PlayerId, c.ChampionKey });

            modelBuilder.Entity<Participant>()
                .HasKey(c => new { c.MatchId, c.PlayerId });

            modelBuilder.Entity<PlayerStats>()
                .HasKey(c => new { c.MatchId, c.PlayerId, c.ChampionKey });

        }

    }
}
