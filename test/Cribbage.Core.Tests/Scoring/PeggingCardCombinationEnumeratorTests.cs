using System;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;
using static Cribbage.Core.Models.Rank;

namespace Cribbage.Core.Tests.Scoring;

public class PeggingCardCombinationEnumeratorTests
{
  [Theory]
  [InlineData(new[] { Five })]
  [InlineData(new[] { Five, Four })]
  [InlineData(new[] { Five, Four, Seven })]
  [InlineData(new[] { Two, Four, King })]
  [InlineData(new[] { Ace })]
  [InlineData(new[] { Jack, Two })]
  [InlineData(new[] { Queen, Three, Ace })]
  [InlineData(new[] { Two, Four, Six, Eight })]
  public void GetCardCombinations_ShouldReturnEmptyWhenNoCombinationsExist(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(r => new Card(Suit.Clubs, r)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Empty(combinations);
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectPair()
  {
    Card[] cardSequence = [new Card(Suit.Clubs, Five), new Card(Suit.Diamonds, Five)];

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Contains(new CardCombination(CombinationType.Pair, cardSequence[^2..]), combinations);
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectThreeOfAKind()
  {
    Card[] cardSequence = [new Card(Suit.Clubs, Three), new Card(Suit.Diamonds, Three), new Card(Suit.Hearts, Three)];

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Contains(new CardCombination(CombinationType.ThreeOfAKind, cardSequence[^3..]), combinations);
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectFourOfAKind()
  {
    Card[] cardSequence = [new Card(Suit.Clubs, Seven), new Card(Suit.Diamonds, Seven), new Card(Suit.Hearts, Seven), new Card(Suit.Spades, Seven)];

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.FourOfAKind));
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Contains(new CardCombination(CombinationType.FourOfAKind, cardSequence[^4..]), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Five, Ten })]
  [InlineData(new[] { Six, Nine })]
  [InlineData(new[] { Seven, Eight })]
  [InlineData(new[] { Ace, Four, Ten })]
  [InlineData(new[] { Two, Three, Ten })]
  [InlineData(new[] { Three, Four, Eight })]
  public void GetCardCombinations_ShouldDetectFifteen(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Fifteen));
      Assert.Contains(new CardCombination(CombinationType.Fifteen, cardSequence), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Five, Ten, Seven, Nine })]
  [InlineData(new[] { Six, Nine, Two, Four, King })]
  [InlineData(new[] { Seven, Eight, Six, Queen })]
  [InlineData(new[] { Ace, Four, Ten, Six, Jack })]
  [InlineData(new[] { Two, Three, Ten, Eight, Ace, Seven })]
  [InlineData(new[] { Three, Four, Eight, Nine, Five, Two })]
  public void GetCardCombinations_ShouldDetectThirtyOne(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.ThirtyOne));
      Assert.Contains(new CardCombination(CombinationType.ThirtyOne, cardSequence), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Four, Three, Five })]
  [InlineData(new[] { Jack, Ten, Nine })]
  [InlineData(new[] { Ten, Jack, Queen })]
  [InlineData(new[] { Ace, Two, Three })]
  public void GetCardCombinations_ShouldDetectRunsOfThree(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, cardSequence[^3..]), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Four, Three, Five, Six })]
  [InlineData(new[] { Ace, Two, Three, Four })]
  [InlineData(new[] { Three, Four, Six, Five })]
  [InlineData(new[] { Two, Three, Ace, Four })]
  public void GetCardCombinations_ShouldDetectRunsOfFour(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, cardSequence[^4..]), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Four, Three, Five, Six, Seven })]
  [InlineData(new[] { Ace, Two, Three, Four, Five })]
  [InlineData(new[] { Three, Four, Six, Five, Two })]
  [InlineData(new[] { Two, Three, Ace, Four, Five })]
  public void GetCardCombinations_ShouldDetectRunsOfFive(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, cardSequence[^5..]), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Four, Three, Five, Six, Seven, Two })]
  [InlineData(new[] { Ace, Two, Three, Four, Six, Five })]
  [InlineData(new[] { Three, Four, Six, Five, Two, Seven })]
  [InlineData(new[] { Two, Three, Ace, Four, Five, Six })]
  public void GetCardCombinations_ShouldDetectRunsOfSix(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, cardSequence[^6..]), combinations);
    });
  }

  [Theory]
  [InlineData(new[] { Two, Three, Ace, Four, Five, Six, Seven })]
  public void GetCardCombinations_ShouldDetectRunsOfSeven(Rank[] ranks)
  {
    Card[] cardSequence = ranks.Select(rank => new Card(Suit.Hearts, rank)).ToArray();

    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, cardSequence[^7..]), combinations);
    });
  }
}
