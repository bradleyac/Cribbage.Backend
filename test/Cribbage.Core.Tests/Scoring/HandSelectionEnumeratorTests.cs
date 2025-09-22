using System;
using System.ComponentModel;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;

namespace Cribbage.Core.Tests.Scoring;

public class HandSelectionEnumeratorTests
{
  [Fact]
  public void GetHandSelections_ShouldReturnAllValidSelections()
  {
    var initialHand = new[]
    {
      new Card(Suit.Hearts, Rank.Ace),
      new Card(Suit.Clubs, Rank.Two),
      new Card(Suit.Diamonds, Rank.Three),
      new Card(Suit.Spades, Rank.King),
      new Card(Suit.Hearts, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
    };

    var selections = HandSelectionEnumerator.GetHandSelections(initialHand).ToList();

    Assert.Equal(15, selections.Count); // 6 choose 2 = 15

    var expectedSelections = new List<(Card[] hand, Card[] discards)>
    {
      (new[] { initialHand[2], initialHand[3], initialHand[4], initialHand[5] }, new[] { initialHand[0], initialHand[1] }),
      (new[] { initialHand[1], initialHand[3], initialHand[4], initialHand[5] }, new[] { initialHand[0], initialHand[2] }),
      (new[] { initialHand[1], initialHand[2], initialHand[4], initialHand[5] }, new[] { initialHand[0], initialHand[3] }),
      (new[] { initialHand[1], initialHand[2], initialHand[3], initialHand[5] }, new[] { initialHand[0], initialHand[4] }),
      (new[] { initialHand[1], initialHand[2], initialHand[3], initialHand[4] }, new[] { initialHand[0], initialHand[5] }),
      (new[] { initialHand[0], initialHand[3], initialHand[4], initialHand[5] }, new[] { initialHand[1], initialHand[2] }),
      (new[] { initialHand[0], initialHand[2], initialHand[4], initialHand[5] }, new[] { initialHand[1], initialHand[3] }),
      (new[] { initialHand[0], initialHand[2], initialHand[3], initialHand[5] }, new[] { initialHand[1], initialHand[4] }),
      (new[] { initialHand[0], initialHand[2], initialHand[3], initialHand[4] }, new[] { initialHand[1], initialHand[5] }),
      (new[] { initialHand[0], initialHand[1], initialHand[4], initialHand[5] }, new[] { initialHand[2], initialHand[3] }),
      (new[] { initialHand[0], initialHand[1], initialHand[3], initialHand[5] }, new[] { initialHand[2], initialHand[4] }),
      (new[] { initialHand[0], initialHand[1], initialHand[3], initialHand[4] }, new[] { initialHand[2], initialHand[5] }),
      (new[] { initialHand[0], initialHand[1], initialHand[2], initialHand[5] }, new[] { initialHand[3], initialHand[4] }),
      (new[] { initialHand[0], initialHand[1], initialHand[2], initialHand[4] }, new[] { initialHand[3], initialHand[5] }),
      (new[] { initialHand[0], initialHand[1], initialHand[2], initialHand[3] }, new[] { initialHand[4], initialHand[5] }),
    };

    Assert.True(expectedSelections.SetwiseEquals(selections, EqualityComparer<(Card[] hand, Card[] discards)>.Create(
      equals: (a, b) => a.hand.SequenceEqual(b.hand) && a.discards.SequenceEqual(b.discards),
      getHashCode: ((Card[] hand, Card[] discards) cards) =>
        cards.hand.Aggregate(0, (acc, card) => acc ^ card.GetHashCode()) ^
        cards.discards.Aggregate(0, (acc, card) => acc ^ card.GetHashCode()))));
  }
}

public static class Extensions
{
  public static bool SetwiseEquals<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer = null)
  {
    var firstSet = new HashSet<T>(first, comparer ?? EqualityComparer<T>.Default);
    return firstSet.SetEquals(second);
  }
}