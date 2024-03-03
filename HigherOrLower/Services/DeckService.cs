using HigherOrLower.Data;
using HigherOrLower.Interface;
using HigherOrLower.Models;
using Microsoft.AspNetCore.Mvc;


namespace HigherOrLower.Services
{
    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        public DeckService(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;
        }

        /// <summary>
        /// Create a deck of cards
        /// </summary>
        /// <returns>returns a deck</returns>
        public Deck CreateDeck()
        {
            Deck newDeck = new Deck
            {
                Cards = new List<Card>()
            };
            foreach (Suits suits in Enum.GetValues(typeof(Suits)))
            {
                foreach (CardValue cardValue in Enum.GetValues(typeof(CardValue)))
                {
                    newDeck.Cards.Add(new Card
                    {
                        CardValue = cardValue,
                        Suits = suits
                    });
                }
            }
            Shuffle(newDeck);
            return newDeck;
        }
        private void Shuffle(Deck deck)
        {
            Random rng = new Random();
            int n = deck.Cards.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = deck.Cards[k];
                deck.Cards[k] = deck.Cards[n];
                deck.Cards[n] = value;
            }
        }

        /// <summary>
        /// Get the first card of the deck
        /// </summary>
        /// <param name="deck">Receive a deck to check if it's finished</param>
        /// <returns>returns a card</returns>
        /// <exception cref="InvalidOperationException">The exception is thrown when the deck is out of cards</exception>
        public Card GetFirstCard(Deck deck)
        {
            if (deck.Cards.Count == 0)
            {
                throw new Exception("No more cards in the deck.");
            }

            var card = deck.Cards[deck.Cards.Count - 1];
            deck.Cards.RemoveAt(deck.Cards.Count - 1);
            return card;
        }

    }
}
