using System;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;

namespace Cribbage.Core.Tests.Scoring;

public class HandStatististicsCalculatorTests
{
  [Fact]
  public void CalculateHandStatistics_ShouldReturnExpectedStatistics()
  {
    Card[] hand = [
      new Card(Suit.Hearts, Rank.Ace),
      new Card(Suit.Clubs, Rank.Two),
      new Card(Suit.Diamonds, Rank.Three),
      new Card(Suit.Spades, Rank.Four),
    ];

    Card[] discards = [
      new Card(Suit.Hearts, Rank.Five),
      new Card(Suit.Clubs, Rank.Six),
    ];

    var statistics = HandStatisticsCalculator.CalculateHandStatistics(hand, discards, ownsCrib: true);

    Assert.Multiple(() =>
    {
      Assert.Equal(10, statistics.HighHandScore);
      Assert.Equal(6, statistics.LowHandScore);

      Assert.Equal(2, statistics.LowCribScore);
      Assert.Equal(24, statistics.HighCribScore);

      Assert.Equal(15.29, statistics.AverageTotalScore);
    });
  }
}
