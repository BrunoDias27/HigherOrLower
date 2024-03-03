using System.Text.Json.Serialization;

namespace HigherOrLower.Models
{
    public class GamePlayer
    {
        [JsonIgnore]
        public int GamePlayerId { get; set; }
        [JsonIgnore]
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int GameId { get; set; }
        [JsonIgnore]
        public Game Game { get; set; }
        public int Points { get; set; }

    }
}
