using HigherOrLower.Interface;
using HigherOrLower.Models;

namespace HigherOrLower.Services
{
    public class PlayersService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayersService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        /// <summary>
        /// Get all the players
        /// </summary>
        /// <returns>Returns a list of players</returns>
        public List<Player> GetAllPlayers()
        {
            return _playerRepository.GetAllPlayers();
        }

    }
}
