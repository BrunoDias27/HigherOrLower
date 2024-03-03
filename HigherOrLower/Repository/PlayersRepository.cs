using HigherOrLower.Data;
using HigherOrLower.Interface;
using HigherOrLower.Models;
using Microsoft.EntityFrameworkCore;

namespace HigherOrLower.Repository
{
    public class PlayersRepository : IPlayerRepository
    {
        public readonly HigherOrLowerContext _higherOrLower;

        public PlayersRepository(HigherOrLowerContext higherOrLower)
        {
            _higherOrLower = higherOrLower;
        }

        public List<Player> GetAllPlayers()
        {
            return _higherOrLower.Player.ToList();
        }

        public Player GetPlayer(int id)
        {
            return _higherOrLower.Player.Where(i => i.PlayerId == id).FirstOrDefault();
        }
    }
}
