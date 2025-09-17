using System;
using System.Numerics;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;
using Xunit.Sdk;

namespace Cribbage.Core.Tests.Scoring;

public class CardCombinationEnumeratorTests
{
  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectPairs(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.Seven)
    ], isCrib);

    Card starter = new Card(Suit.Clubs, Rank.King);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(2, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Contains(new CardCombination(CombinationType.Pair, [hand.Cards[0], hand.Cards[1]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Pair, [hand.Cards[2], hand.Cards[3]]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectPairsIncludingStarter(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.King)
    ], isCrib);

    Card starter = new Card(Suit.Clubs, Rank.Seven);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(2, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Contains(new CardCombination(CombinationType.Pair, [hand.Cards[0], hand.Cards[1]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Pair, [hand.Cards[2], starter]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectThreeOfAKind(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Five),
      new Card(Suit.Spades, Rank.Seven)
    ], isCrib);

    Card starter = new Card(Suit.Clubs, Rank.King);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Contains(new CardCombination(CombinationType.ThreeOfAKind, [hand.Cards[0], hand.Cards[1], hand.Cards[2]]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectThreeOfAKindIncludingStarter(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.King)
    ], isCrib);

    Card starter = new Card(Suit.Spades, Rank.Five);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Contains(new CardCombination(CombinationType.ThreeOfAKind, [hand.Cards[0], hand.Cards[1], starter]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectFourOfAKind(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Five),
      new Card(Suit.Spades, Rank.Five)
    ], isCrib);

    Card starter = new Card(Suit.Clubs, Rank.King);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.FourOfAKind));
      Assert.Contains(new CardCombination(CombinationType.FourOfAKind, [hand.Cards[0], hand.Cards[1], hand.Cards[2], hand.Cards[3]]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectFourOfAKindIncludingStarter(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Five),
      new Card(Suit.Spades, Rank.King)
    ], isCrib);

    Card starter = new Card(Suit.Spades, Rank.Five);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Pair));
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.ThreeOfAKind));
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.FourOfAKind));
      Assert.Contains(new CardCombination(CombinationType.FourOfAKind, [hand.Cards[0], hand.Cards[1], hand.Cards[2], starter]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectFifteens(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Five),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.King)
    ], isCrib);

    Card starter = new Card(Suit.Hearts, Rank.Five);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(4, combinations.Count(c => c.Type == CombinationType.Fifteen));
      Assert.Contains(new CardCombination(CombinationType.Fifteen, [hand.Cards[3], starter]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Fifteen, [hand.Cards[0], hand.Cards[3]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Fifteen, [hand.Cards[1], hand.Cards[3]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Fifteen, [hand.Cards[0], hand.Cards[1], starter]), combinations);
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectFlushesInHandWithoutStarter()
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
      new Card(Suit.Clubs, Rank.Seven),
      new Card(Suit.Clubs, Rank.King)
    ], false);

    Card starter = new Card(Suit.Diamonds, Rank.Eight);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Flush));
      Assert.Contains(new CardCombination(CombinationType.Flush, hand.Cards), combinations);
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectFlushesInHandWithStarter()
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
      new Card(Suit.Clubs, Rank.Seven),
      new Card(Suit.Clubs, Rank.King)
    ], false);

    Card starter = new Card(Suit.Clubs, Rank.Eight);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Flush));
      Assert.Contains(new CardCombination(CombinationType.Flush, [.. hand.Cards, starter]), combinations);
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldNotDetectFlushesInCribWithoutStarter()
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
      new Card(Suit.Clubs, Rank.Seven),
      new Card(Suit.Clubs, Rank.King)
    ], true);

    Card starter = new Card(Suit.Diamonds, Rank.Eight);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(0, combinations.Count(c => c.Type == CombinationType.Flush));
    });
  }

  [Fact]
  public void GetCardCombinations_ShouldDetectFlushesInCribWithStarter()
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
      new Card(Suit.Clubs, Rank.Seven),
      new Card(Suit.Clubs, Rank.King)
    ], true);

    Card starter = new Card(Suit.Clubs, Rank.Eight);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Flush));
      Assert.Contains(new CardCombination(CombinationType.Flush, [.. hand.Cards, starter]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectThreeCardRuns(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Six),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.Five)
    ], isCrib);

    Card starter = new Card(Suit.Hearts, Rank.Five);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(3, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, [hand.Cards[0], hand.Cards[1], hand.Cards[2]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Run, [hand.Cards[3], hand.Cards[1], hand.Cards[2]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Run, [starter, hand.Cards[1], hand.Cards[2]]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectFourCardRuns(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Six),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.Eight)
    ], isCrib);

    Card starter = new Card(Suit.Hearts, Rank.Five);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(2, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, [hand.Cards[0], hand.Cards[1], hand.Cards[2], hand.Cards[3]]), combinations);
      Assert.Contains(new CardCombination(CombinationType.Run, [starter, hand.Cards[1], hand.Cards[2], hand.Cards[3]]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectFiveCardRuns(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Six),
      new Card(Suit.Hearts, Rank.Seven),
      new Card(Suit.Spades, Rank.Eight)
    ], isCrib);

    Card starter = new Card(Suit.Hearts, Rank.Nine);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Run));
      Assert.Contains(new CardCombination(CombinationType.Run, [hand.Cards[0], hand.Cards[1], hand.Cards[2], hand.Cards[3], starter]), combinations);
    });
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void GetCardCombinations_ShouldDetectNobs(bool isCrib)
  {
    Hand hand = new Hand(
    [
      new Card(Suit.Clubs, Rank.Five),
      new Card(Suit.Diamonds, Rank.Six),
      new Card(Suit.Hearts, Rank.Jack),
      new Card(Suit.Spades, Rank.Eight)
    ], isCrib);

    Card starter = new Card(Suit.Hearts, Rank.Nine);

    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter).ToList();

    Assert.Multiple(() =>
    {
      Assert.Equal(1, combinations.Count(c => c.Type == CombinationType.Nobs));
      Assert.Contains(new CardCombination(CombinationType.Nobs, [hand.Cards[2]]), combinations);
    });
  }
}
