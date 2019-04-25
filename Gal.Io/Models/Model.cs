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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasIndex(b => b.SummonerName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(b => b.Username)
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

    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }

}
