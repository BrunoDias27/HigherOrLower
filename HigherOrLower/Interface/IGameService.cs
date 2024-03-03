using HigherOrLower.Models;
using static HigherOrLower.Services.GameService;

namespace HigherOrLower.Interface
{
    public interface IGameService
    {
        Game StartNewGame();

        Card GetPlayedCard(int id);

        GameResult MakeGuess(string guess, int id);

        Player GetPlayerWhosNext(int id);

        bool IsFinished(Game game);
        List<Game> GetAllAvailableGames();

        List<GamePlayer> GetGameResult(int id);
    }
}
