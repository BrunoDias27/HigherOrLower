
using Microsoft.AspNetCore.Mvc;
using HigherOrLower.Interface;

namespace HigherOrLower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <returns>
        /// Returns a game
        /// </returns>
        [HttpPost("CreateNewGame")]
        public IActionResult StartNewGame()
        {
            var game = _gameService.StartNewGame();
            return Ok(new { game, Player = _gameService.GetPlayerWhosNext(game.GameId)  });
        }

        /// <summary>
        /// Get the card currently in play
        /// </summary>
        /// <param name="gameId"> Request's a GameId</param>
        /// <returns>Returns the card currently in play</returns>
        [HttpGet("CheckGameCard")]
        public IActionResult GetGameCard([FromQuery] int gameId) {
            try
            {
                var games = _gameService.GetPlayedCard(gameId);
                return Ok(games);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Method to play the game
        /// </summary>
        /// <param name="id">Request's a GameId</param>
        /// <param name="guess">Request's a guess for the game</param>
        /// <returns>Returns the game</returns>
        [HttpPost("Play")]
        public IActionResult Play([FromQuery] int id, string guess)
        {
            try
            {
                var game = _gameService.MakeGuess(guess, id);
                if (_gameService.IsFinished(game.Game))
                    return Ok(new { game, game.Result, GameStatus = "Game is finished", GameScore = _gameService.GetGameResult(id) });

                return Ok(new { game.Game, game.Result });
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Check who's next to play
        /// </summary>
        /// <param name="gameId">Request's a GameId</param>
        /// <returns>returns the player who is playing next</returns>
        [HttpGet("WhosNextToPlay")]
        public IActionResult GetWhosNextToPlay(int gameId)
        {
            try
            {
                var player = _gameService.GetPlayerWhosNext(gameId);
                return Ok(player);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Check all available games to play
        /// </summary>
        /// <returns>Returns all games available</returns>
        [HttpGet("AllAvailableGames")]
        public IActionResult GetAllAvailableGames()
        {
            try
            {
                var game = _gameService.GetAllAvailableGames();
                return Ok(game);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Verify the result from the game
        /// </summary>
        /// <param name="gameId">Request's a GameId</param>
        /// <returns>Return the game result</returns>
        [HttpGet("CheckGameResult")]
        public IActionResult GetGameResults(int gameId)
        {
            try
            {
                var game = _gameService.GetGameResult(gameId);
                return Ok(game);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
