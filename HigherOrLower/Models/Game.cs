using System.Text.Json.Serialization;

namespace HigherOrLower.Models
{
    public class Game
    {
        public int GameId { get; set; }

        [JsonIgnore]
        public Deck Deck { get; set; }
        [JsonIgnore]
        public List<GamePlayer> Players { get; set; }
        
        public Card PlayedCard { get; set; }
        [JsonIgnore]
        public int CurrentPlayerIndex { get; set; }

    }
}
