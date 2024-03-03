using System.Text.Json.Serialization;

namespace HigherOrLower.Models
{
    public class Player
    {
        [JsonIgnore]
        public int PlayerId { get; set; }
        public required string NamePlayer { get; set; }
        [JsonIgnore]
        public List<GamePlayer> ? GamePlayer { get; set; }
        
    }
}
