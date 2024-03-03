using HigherOrLower.Interface;
using HigherOrLower.Models;
using Microsoft.EntityFrameworkCore;

namespace HigherOrLower.Services
{
    public class GameService : IGameService
    {
        private readonly IDeckService _deckService;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameService(IDeckService deckService, IGameRepository gameRepository, IPlayerRepository playerRepository) {
            _deckService = deckService;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }


        /// <summary>
        /// The game is created together with a deck of cards and associated the players
        /// </summary>
        /// <returns>
        /// Returns a Game
        /// </returns>
        public Game StartNewGame()
        {
            var players = _playerRepository.GetAllPlayers();
            Game game = new Game
            {
                Deck = _deckService.CreateDeck(),
                Players = new List<GamePlayer>(),
            };

            foreach (var player in players)
            {
                var gamePlayers = new GamePlayer
                {
                    GameId = game.GameId,
                    PlayerId = player.PlayerId,
                    Points = 0
                };
                game.Players.Add(gamePlayers);
            }
            game.CurrentPlayerIndex = game.Players.FirstOrDefault().PlayerId - 1;
            game.PlayedCard = _deckService.GetFirstCard(game.Deck);
            _gameRepository.StartGame(game);
            return game;
        }

        /// <summary>
        /// GetPlayedCard returns the card that is in play
        /// </summary>
        /// <param name="id">Receive an id to get the game</param>
        /// <returns>
        /// The card in play
        /// </returns>
        /// <exception cref="Exception"> The exception is thrown when the game is Over </exception>
        public Card GetPlayedCard(int id)
        {
            var game = _gameRepository.GetGame(id);
            if (IsFinished(game)) throw new Exception("The game is over!! Try other Game");
            return game.PlayedCard;
        }

        /// <summary>
        /// GetAllGames returns all the games
        /// </summary>
        /// <returns> returns all the games </returns>
        /// <exception cref="Exception"> The exception is thrown when there are no games </exception>
        public List<Game> GetAllGames()
        {
            var game = _gameRepository.GetAllGames();
            if (game.Count < 1 || game == null) throw new Exception("0 Games Available");
            return _gameRepository.GetAllGames();
        }

        /// <summary>
        /// MakeGuess will reacive the player's guess and play the game
        /// </summary>
        /// <param name="guess">Player guess for the game</param>
        /// <param name="id">Id received from the user to get the game</param>
        /// <returns>Returns a gameresult with the game and the result </returns>
        /// <exception cref="Exception">The exception is thrown when the defined game has already ended </exception>
        /// <exception cref="Exception">The exception is thrown when the user use invalid keywords</exception>
        public GameResult MakeGuess(string guess, int id)
        {
            string result  ="Incorrect";
            var game = _gameRepository.GetGame(id);

            if(IsFinished(game)) throw new Exception("Game is Already Finishided");

            if (!ValidateGuess(guess)) throw new Exception("Wrong KeyWord - try Lower or Higher");
            ReloadPlayers(game);
            var NextCard = _deckService.GetFirstCard(game.Deck);
            if (GuessLowerIsCorrect(NextCard,game,guess) || GuessHigherIsCorrect(NextCard, game, guess))
            {
                game.Players[game.CurrentPlayerIndex].Points++;
                result = "Correct";
            }
            game.CurrentPlayerIndex++;
            game.PlayedCard = NextCard;
            _gameRepository.MakeGuess(game);

            return new GameResult { Game = game, Result =  result };
        }


        /// <summary>
        /// GetPlayerWhosNext will return the next player who will play the game
        /// </summary>
        /// <param name="id">Id received from the user to get the game</param>
        /// <returns> return the next player </returns>
        /// <exception cref="Exception">The exception is thrown when the game is already finishided</exception>
        public Player GetPlayerWhosNext(int id)
        {
            var game = _gameRepository.GetGame(id);
            if(IsFinished(game)) throw new Exception("Game is Already Finishided");
            ReloadPlayers(game);
            var player = _playerRepository.GetPlayer(game.CurrentPlayerIndex + 1);
            return player;
        }

        /// <summary>
        /// GetAllAvailableGames will return all the games available
        /// </summary>
        /// <returns> returns all the games available </returns>
        /// <exception cref="Exception">The exception is thrown when their aren't any game available</exception>
        /// <exception cref="Exception">The exception is thrown when all the games have already ended</exception>
        public List<Game> GetAllAvailableGames()
        {
            var allGames = _gameRepository.GetAllGames();
            if (allGames.Count < 1) throw new Exception("0 Games Available");

            List<Game> allGamesAvailable = new List<Game>();
            
            for(int i = 0; i < allGames.Count; i++)
            {
                if (!IsFinished(allGames[i]))
                    allGamesAvailable.Add(allGames[i]);
            }
            if(allGamesAvailable.Count < 1) throw new Exception("All the games are finished");
            return allGamesAvailable;
        }

        /// <summary>
        /// GetGameResult will return the scores of the players
        /// </summary>
        /// <param name="id">Id received from the user to get the game</param>
        /// <returns>Returns the players with the game results</returns>
        public List<GamePlayer> GetGameResult(int id)
        {
            var game = _gameRepository.GetGame(id);
            var gamePlayers = game.Players
                .ToList();

            return gamePlayers;
        }
        private bool IsLower(string str)
        {
            return str.ToLower() == "lower";
        }

        private bool IsHigher(string str)
        {
            return str.ToLower() == "higher";
        }

        private bool ValidateGuess(string str)
        {
            return IsLower(str) || IsHigher(str); 
        }

        private bool GuessLowerIsCorrect(Card card, Game game,string guess)
        {
            return IsLower(guess) && card.CardValue < game.PlayedCard.CardValue;
        }

        private bool GuessHigherIsCorrect(Card card, Game game, string guess)
        {
            return IsHigher(guess) && card.CardValue > game.PlayedCard.CardValue || card.CardValue == game.PlayedCard.CardValue;
        }

        private void ReloadPlayers(Game game)
        {
            if (game.CurrentPlayerIndex < _playerRepository.GetAllPlayers().Count()) return;
            game.CurrentPlayerIndex = 0;
        }

        /// <summary>
        /// Verify if the game is finished
        /// </summary>
        /// <param name="game">Receive a game to check if it's finished</param>
        /// <returns>
        /// True = Finished
        /// False = Not finished
        /// </returns>
        public bool IsFinished(Game game)
        {
            return game.Deck.Cards.Count < 1;
        }
    }
}
