using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Gal.Io.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasIndex(b => b.SummonerName)
                .IsUnique();
        }

    }

    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SummonerName { get; set; }
        public string Notes { get; set; }

    }

}
