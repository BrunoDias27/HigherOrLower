using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HigherOrLower.Models;
using HigherOrLower.Controllers;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace HigherOrLower.Data
{
    public class HigherOrLowerContext : DbContext
    {

        public HigherOrLowerContext(DbContextOptions<HigherOrLowerContext> options)
            : base(options)
        {
        }
        //adicionar os 2 jogadores aqui
        public DbSet<Game> Game { get; set; }
        public DbSet<Player> Player { get; set; }

        public DbSet<GamePlayer> GamePlayers { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Deck> Deck { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Player zei = new Player
            {
                PlayerId = 1,
                NamePlayer = "Player1"
            };

            Player quim = new Player
            {
                PlayerId = 2,
                NamePlayer = "Player2"
            };

            modelBuilder.Entity<Player>().HasData(zei, quim);
        }

    }
}
