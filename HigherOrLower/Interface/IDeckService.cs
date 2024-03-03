using Microsoft.AspNetCore.Mvc;
using HigherOrLower.Models;
namespace HigherOrLower.Interface
{
    public interface IDeckService
    {
        Deck CreateDeck();

        Card GetFirstCard(Deck deck);

    }
}
