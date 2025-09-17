using System;

namespace Cribbage.Core.Models.Tests;

public class DeckTests
{
  [Fact]
  public void Ctor_ShouldCreateDeckWith52Cards()
  {
    var deck = new Deck();

    Assert.Equal(52, deck.Cards.Count);
  }

  [Fact]
  public void Ctor_ShouldCreateDeckWithAllUniqueCards()
  {
    var deck = new Deck();

    var uniqueCards = new HashSet<Card>(deck.Cards);
    Assert.Equal(deck.Cards.Count, uniqueCards.Count);
  }

  [Fact]
  public void Ctor_ShouldRandomizeCardOrder()
  {
    var deck = new Deck();
    var deck2 = new Deck();

    Assert.False(Deck.DefaultCards.SequenceEqual(deck.Cards), "Deck should not be in default order");
    Assert.False(Deck.DefaultCards.SequenceEqual(deck2.Cards), "Deck2 should not be in default order");
    Assert.False(deck.Cards.SequenceEqual(deck2.Cards), "Two newly created decks should not be in the same order");
  }

  [Fact]
  public void Shuffle_ShouldRandomizeCardOrder()
  {
    var deck = new Deck();
    var originalOrder = deck.Cards.ToList();

    deck.Shuffle();

    Assert.False(originalOrder.SequenceEqual(deck.Cards), "Deck should not be in original order after shuffle");
  }
}
