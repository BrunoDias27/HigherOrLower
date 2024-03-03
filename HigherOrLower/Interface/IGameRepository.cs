using HigherOrLower.Models;

namespace HigherOrLower.Interface
{
    public interface IGameRepository
    {
        bool StartGame(Game game);

        List<Game> GetAllGames();

        Game GetGame(int id);

        bool MakeGuess(Game game);
    }
}
