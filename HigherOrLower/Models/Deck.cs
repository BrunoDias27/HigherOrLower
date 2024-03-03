using System.Text.Json.Serialization;

namespace HigherOrLower.Models
{
    public class Deck
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<Card> Cards { get; set; }


    }
}
