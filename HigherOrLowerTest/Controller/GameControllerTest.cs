using FakeItEasy;
using FluentAssertions;
using HigherOrLower.Controllers;
using HigherOrLower.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AutoFixture;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using HigherOrLower.Interface;

namespace HigherOrLowerTest.Controller
{
    public class GameControllerTest
    {
        private readonly IGameService _gameService;
        //private readonly Mock<IGameService> _gameServiceMock;

        public GameControllerTest()
        {
            _gameService = A.Fake<IGameService>();
            //_deckServiceMock = new Mock<IDeckService>();

        }


        [Fact]
        public void GamesController_GetAllAvailableGames_ReturnOK()
        {
            //Arrange
            var games = A.Fake<List<Game>>();
            A.CallTo(() => _gameService.GetAllAvailableGames()).Returns(games);

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetAllAvailableGames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_startNewGame_ReturnOK()
        {
            //Arrange
            var games = A.Fake<List<Game>>();
            A.CallTo(() => _gameService.GetAllAvailableGames()).Returns(games);

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetAllAvailableGames();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_GetGameCard_ReturnOk()
        {
            //Arrange
            var game = A.Fake<Game>();
            A.CallTo(() => _gameService.GetPlayedCard(game.GameId)).Returns(game.PlayedCard);

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetGameCard(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_GetGameCard_ReturnBadRequest()
        {
            //Arrange
            var game = A.Fake<Game>();
            A.CallTo(() => _gameService.GetPlayedCard(game.GameId)).Throws<Exception>();

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetGameCard(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GamesController_Play_ReturnOk()
        {
            //Arrange
            var game = A.Fake<Game>();
            var gameResult = A.Fake<GameResult>();
            string guess = "Lower";
            A.CallTo(() => _gameService.MakeGuess(guess, game.GameId)).Returns(gameResult);
            A.CallTo(() => _gameService.IsFinished(game)).Returns(false);

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.Play(game.GameId,guess);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_Play_ReturnBadRequest()
        {
            //Arrange
            var game = A.Fake<Game>();
            var gameResult = A.Fake<GameResult>();
            string guess = "Lower";
            A.CallTo(() => _gameService.MakeGuess(guess, game.GameId)).Throws<Exception>();

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.Play(game.GameId, guess);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GamesController_GetWhosNextToPlay_ReturnOk()
        {
            //Arrange
            var game = A.Fake<Game>();

            A.CallTo(() => _gameService.GetPlayerWhosNext(game.GameId)).Returns(new Player { NamePlayer = "player3"});



            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetWhosNextToPlay(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_GetWhosNextToPlay_ReturnBadRequest()
        {
            //Arrange
            var game = A.Fake<Game>();

            A.CallTo(() => _gameService.GetPlayerWhosNext(game.GameId)).Throws<Exception>();

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetWhosNextToPlay(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GamesController_GetGameResults_ReturnOk()
        {
            //Arrange
            var game = A.Fake<Game>();
            var gamePlayer = A.Fake<List<GamePlayer>>();
            A.CallTo(() => _gameService.GetGameResult(game.GameId)).Returns(gamePlayer);

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetGameResults(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void GamesController_GetGameResults_ReturnBadRequest()
        {
            //Arrange
            var game = A.Fake<Game>();
            var gamePlayer = A.Fake<List<GamePlayer>>();
            A.CallTo(() => _gameService.GetGameResult(game.GameId)).Throws<Exception>();

            var controller = new GamesController(_gameService);

            //Act
            var result = controller.GetGameResults(game.GameId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
