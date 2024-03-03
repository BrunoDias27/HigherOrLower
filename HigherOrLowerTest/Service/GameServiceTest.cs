using HigherOrLower.Data;
using HigherOrLower.Interface;
using HigherOrLower.Models;
using HigherOrLower.Services;
using Moq;


namespace HigherOrLowerTest.Service
{
    public class GameServiceTest
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IDeckService> _deckServiceMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        public GameServiceTest()
        {

            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _deckServiceMock = new Mock<IDeckService>();
            _gameRepositoryMock = new Mock<IGameRepository>();
        }

        [Fact]
        public void StartNewGame_ok()
        {
            //Arrange
            _playerRepositoryMock.Setup(repo => repo.GetAllPlayers())
                                .Returns(new List<Player>
                                {
                                new Player { PlayerId = 1 ,NamePlayer = "Player1"},
                                new Player { PlayerId = 2 ,NamePlayer = "Player2"}
                                });

            _deckServiceMock.Setup(service => service.CreateDeck())
                           .Returns(new Deck());

            _deckServiceMock.Setup(service => service.GetFirstCard(It.IsAny<Deck>()))
                       .Returns(new Card());


            var gameService = new GameService(_deckServiceMock.Object,
                                          _gameRepositoryMock.Object,
                                          _playerRepositoryMock.Object);
            //Act

            var result = gameService.StartNewGame();
            //Assert

            Assert.NotNull(result);
            Assert.NotNull(result.PlayedCard);
            Assert.NotNull(result.Deck);
            _gameRepositoryMock.Verify(r => r.StartGame(result), Times.Once);

        }


        [Fact]
        public void GetPlayedCard_OK()
        {
            // Arrange
            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };
            
            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck { Cards = new List<Card> { new Card(), new Card(), new Card() } },
                PlayedCard = new Card(),
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            var gameService = new GameService(_deckServiceMock.Object,
                                              _gameRepositoryMock.Object,
                                               _playerRepositoryMock.Object);

            // Act
            var resultCard = gameService.GetPlayedCard(game.GameId);

            // Assert
            Assert.Equal(game.PlayedCard, resultCard);
        }

        [Fact]
        public void GetPlayedCard_GameIsOverReturnException()
        {
            //Arrange

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck { Cards = new List<Card>() },
                PlayedCard = new Card(),
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            var gameService = new GameService(_deckServiceMock.Object,
                                              _gameRepositoryMock.Object,
                                               _playerRepositoryMock.Object);

            string expectedExceptionMessage = "The game is over!! Try other Game";
            //Act And Assert
            var exception = Assert.Throws<Exception>(() => gameService.GetPlayedCard(game.GameId));
            Assert.Equal(expectedExceptionMessage, exception.Message);

        }

        [Fact]
        public void GetAllGames_OK()
        {

            _gameRepositoryMock.Setup(repo => repo.GetAllGames())
                              .Returns(new List<Game>{ new Game() });

            var gameService = new GameService(_deckServiceMock.Object,
                                              _gameRepositoryMock.Object,
                                               _playerRepositoryMock.Object);

            var result = gameService.GetAllGames();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void GetAllGames_ZeroGames()
        {
            //Arrange
            _gameRepositoryMock.Setup(repo => repo.GetAllGames())
                              .Returns(new List<Game>());

            var gameService = new GameService(_deckServiceMock.Object,
                                              _gameRepositoryMock.Object,
                                               _playerRepositoryMock.Object);



            //Act And Assert
            string expectedExceptionMessage = "0 Games Available";
            var exception = Assert.Throws<Exception>(() => gameService.GetAllGames());
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void MakeGuess_Ok()
        {
            //Arrange
            string guess = "Lower";

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck { Cards = new List<Card> { new Card { CardValue= CardValue.Five,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Ace,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Jack,
                Suits = Suits.Hearts} } },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            _deckServiceMock.Setup(repo => repo.GetFirstCard(game.Deck))
                              .Returns(new Card());

            _playerRepositoryMock.Setup(repo => repo.GetAllPlayers())
                    .Returns(new List<Player>
                    {
                                new Player { PlayerId = 1 ,NamePlayer = "Player1"},
                                new Player { PlayerId = 2 ,NamePlayer = "Player2"}
                    });
            var gameService = new GameService(_deckServiceMock.Object,
                                  _gameRepositoryMock.Object,
                                   _playerRepositoryMock.Object);


            var result = gameService.MakeGuess(guess,game.GameId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(game.GameId, result.Game.GameId);
            if (result.Result == "Correct")
            {
                Assert.Equal("Correct", result.Result);
            }
            else
            {
                Assert.Equal("Incorrect", result.Result);
            }
            
        }

        [Fact]
        public void MakeGuess_InvalidGuess()
        {
            //Arrange
            string guess = "Wrong";

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card> { new Card { CardValue= CardValue.Five,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Ace,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Jack,
                Suits = Suits.Hearts} }
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            _deckServiceMock.Setup(repo => repo.GetFirstCard(game.Deck))
                              .Returns(new Card());

            _playerRepositoryMock.Setup(repo => repo.GetAllPlayers())
                    .Returns(new List<Player>
                    {
                                new Player { PlayerId = 1 ,NamePlayer = "Player1"},
                                new Player { PlayerId = 2 ,NamePlayer = "Player2"}
                    });
            var gameService = new GameService(_deckServiceMock.Object,
                                  _gameRepositoryMock.Object,
                                   _playerRepositoryMock.Object);

            //Act And Assert
            string expectedExceptionMessage = "Wrong KeyWord - try Lower or Higher";
            var exception = Assert.Throws<Exception>(() => gameService.MakeGuess(guess,game.GameId));
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void MakeGuess_GameIsFinished()
        {
            //Arrange
            string guess = "Wrong";

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card>()
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            _deckServiceMock.Setup(repo => repo.GetFirstCard(game.Deck))
                              .Returns(new Card());

            _playerRepositoryMock.Setup(repo => repo.GetAllPlayers())
                    .Returns(new List<Player>
                    {
                                new Player { PlayerId = 1 ,NamePlayer = "Player1"},
                                new Player { PlayerId = 2 ,NamePlayer = "Player2"}
                    });
            var gameService = new GameService(_deckServiceMock.Object,
                                  _gameRepositoryMock.Object,
                                   _playerRepositoryMock.Object);

            //Act And Assert
            string expectedExceptionMessage = "Game is Already Finishided";
            var exception = Assert.Throws<Exception>(() => gameService.MakeGuess(guess, game.GameId));
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void GetAllGamesAvailable_Ok()
        {
            //Arrange

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card> { new Card { CardValue= CardValue.Five,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Ace,
                Suits = Suits.Diamons}, new Card{ CardValue= CardValue.Jack,
                Suits = Suits.Hearts} }
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetAllGames())
                              .Returns(new List<Game> { game });

            var gameService = new GameService(_deckServiceMock.Object,
                      _gameRepositoryMock.Object,
                       _playerRepositoryMock.Object);
            //Act
            var result = gameService.GetAllAvailableGames();

            //Assert
            Assert.NotNull(result);
            Assert.Contains(game, result);
        }

        [Fact]
        public void GetAllGamesAvailable_ThrowExceptionIfGameIsFinished()
        {
            //Arrange

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card>()
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetAllGames())
                              .Returns(new List<Game> { game });


            var gameService = new GameService(_deckServiceMock.Object,
                      _gameRepositoryMock.Object,
                       _playerRepositoryMock.Object);

            //Act And Assert
            string expectedExceptionMessage = "All the games are finished";
            var exception = Assert.Throws<Exception>(() => gameService.GetAllAvailableGames());
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void GetAllGamesAvailable_ThrowExceptionWhenDontHaveAvailableGames()
        {
            //Arrange


            _gameRepositoryMock.Setup(repo => repo.GetAllGames())
                              .Returns(new List<Game>());


            var gameService = new GameService(_deckServiceMock.Object,
                      _gameRepositoryMock.Object,
                       _playerRepositoryMock.Object);

            //Act And Assert
            string expectedExceptionMessage = "0 Games Available";
            var exception = Assert.Throws<Exception>(() => gameService.GetAllAvailableGames());
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void GetPlayerWhosNext_ThrowExceptionWhenGameIsFinished()
        {
            //Arrange

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card>()
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                              .Returns(game);

            _playerRepositoryMock.Setup(repo => repo.GetAllPlayers())
                            .Returns(new List<Player>
                            {
                                            new Player { PlayerId = 1 ,NamePlayer = "Player1"},
                                            new Player { PlayerId = 2 ,NamePlayer = "Player2"}
                            });



            var gameService = new GameService(_deckServiceMock.Object,
                                            _gameRepositoryMock.Object,
                                            _playerRepositoryMock.Object);

            //Act And Assert
            string expectedExceptionMessage = "Game is Already Finishided";
            var exception = Assert.Throws<Exception>(() => gameService.GetPlayerWhosNext(game.GameId));
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }


        [Fact]
        public void GetGameResult()
        {
            //Arrange

            Player player1 = new Player { PlayerId = 1, NamePlayer = "Player1" };
            Player player2 = new Player { PlayerId = 2, NamePlayer = "Player2" };

            Game game = new Game
            {
                GameId = 1,
                CurrentPlayerIndex = 1,
                Deck = new Deck
                {
                    Cards = new List<Card>()
                },
                PlayedCard = new Card
                {
                    CardValue = CardValue.Two,
                    Suits = Suits.Diamons
                },
                Players = new List<GamePlayer>
                {
                    new GamePlayer { PlayerId = 1, GameId = 1 ,Points = 0, GamePlayerId = player1.PlayerId,},
                    new GamePlayer { PlayerId = 2, GameId = 1 ,Points = 0, GamePlayerId = player2.PlayerId,}
                }

            };

            _gameRepositoryMock.Setup(repo => repo.GetGame(game.GameId))
                  .Returns(game);

            var gameService = new GameService(_deckServiceMock.Object,
                                _gameRepositoryMock.Object,
                                _playerRepositoryMock.Object);

            //Act
            var result = gameService.GetGameResult(game.GameId);

            //Assert
            Assert.NotNull(result);

        }

    }
}
