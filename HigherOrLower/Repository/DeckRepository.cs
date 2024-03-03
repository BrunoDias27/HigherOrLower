using HigherOrLower.Data;
using HigherOrLower.Interface;
using HigherOrLower.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HigherOrLower.Repository
{
    public class DeckRepository : IDeckRepository
    {
        public readonly HigherOrLowerContext _higherOrLower;
        public DeckRepository(HigherOrLowerContext higherOrLower) {
            _higherOrLower = higherOrLower;
        }

        public List<Card> GetAllCards(List<Card> decks)
        {
            return decks;
        }


        public async Task<bool> CreateDeck(Deck deck) {
            _higherOrLower.Add(deck);
            await _higherOrLower.SaveChangesAsync();
            return true;
        }

    }
}
