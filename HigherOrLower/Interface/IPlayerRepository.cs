using HigherOrLower.Models;

namespace HigherOrLower.Interface
{
    public interface IPlayerRepository
    {
        List<Player> GetAllPlayers();

        Player GetPlayer(int id);
    }
}
