
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace HigherOrLower.Models
{
    public class Card
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CardValue CardValue { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Suits Suits { get; set; }


    }
}
