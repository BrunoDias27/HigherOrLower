using HigherOrLower.Models;

namespace HigherOrLower.Interface
{
    public interface IDeckRepository
    {
        List<Card> GetAllCards(List<Card> decks);

        Task<bool> CreateDeck(Deck deck);
    }
}
