using HigherOrLower.Data;
using HigherOrLower.Interface;
using HigherOrLower.Models;
using Microsoft.EntityFrameworkCore;

namespace HigherOrLower.Repository
{
    public class GameRepository : IGameRepository
    {
        public readonly HigherOrLowerContext _higherOrLowerContext;
        public GameRepository(HigherOrLowerContext higherOrLowerContext) {
            _higherOrLowerContext = higherOrLowerContext;
        }

        public bool StartGame(Game game)
        {
            _higherOrLowerContext.Add(game);
            _higherOrLowerContext.SaveChanges();
            return true;
        }


        public List<Game> GetAllGames()
        {
            var games = _higherOrLowerContext.Game
                    .Include(p => p.Players)
                    .Include(d => d.Deck)
                        .ThenInclude(c => c.Cards)
                    .Include(f => f.PlayedCard).ToList();

            return games;
        }

        public Game GetGame(int id)
        {
            var game = _higherOrLowerContext.Game
                .Include(p => p.Players)
                    .ThenInclude(p => p.Player)
                .Include(d => d.Deck)
                    .ThenInclude(c => c.Cards)
                .Include(f => f.PlayedCard)
                .FirstOrDefault(g => g.GameId == id);

            if (game == null)
                throw new Exception("Error ! Try a valid game Id");

            return game;
        }

        public bool MakeGuess(Game game)
        {
            _higherOrLowerContext.SaveChanges();
            return true;
        }


    }
}
