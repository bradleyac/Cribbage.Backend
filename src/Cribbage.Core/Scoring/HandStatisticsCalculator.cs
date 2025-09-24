using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class HandStatisticsCalculator
{
  public record HandStatistics(int LowHandScore, int HighHandScore, double AverageHandScore, double AverageCribScore) : IEquatable<HandStatistics>
  {
    public virtual bool Equals(HandStatistics? other) => other is not null &&
      LowHandScore == other.LowHandScore &&
      HighHandScore == other.HighHandScore &&
      Math.Abs(AverageHandScore - other.AverageHandScore) < 0.01 &&
      Math.Abs(AverageCribScore - other.AverageCribScore) < 0.01;

    public override int GetHashCode() => HashCode.Combine(LowHandScore, HighHandScore, Math.Round(AverageHandScore, 1), Math.Round(AverageCribScore, 1));
  };

  public static HandStatistics CalculateHandStatistics(Card[] hand, Card[] discards)
  {
    var remainingDeck = Utils.GetComplement([.. hand, .. discards]);

    int totalHandScore = 0;
    int lowHandScore = int.MaxValue;
    int highHandScore = 0;
    int totalCribScore = 0;

    for (int i = 0; i < remainingDeck.Length; i++)
    {
      var cutCard = remainingDeck[i];
      var rest = remainingDeck.Except([cutCard]).ToArray();

      var handScore = ScoreCalculator.CalculateScore(CardCombinationEnumerator.GetCardCombinations(new Hand(hand, IsCrib: false), cutCard));
      if (handScore < lowHandScore) lowHandScore = handScore;
      if (handScore > highHandScore) highHandScore = handScore;
      totalHandScore += handScore;

      foreach (var cribCards in Utils.NChooseC(rest, 2))
      {
        var cribHand = new Hand([.. discards, .. cribCards], IsCrib: true);
        var cribScore = ScoreCalculator.CalculateScore(CardCombinationEnumerator.GetCardCombinations(cribHand, cutCard));

        var sorted = cribHand.Cards.OrderBy(c => c.Rank).ThenBy(c => c.Suit).ToArray();

        totalCribScore += cribScore;
      }
    }

    int numberOfHands = remainingDeck.Length;
    int numberOfCribs = numberOfHands * (remainingDeck.Length - 1) * (remainingDeck.Length - 2) / 2;

    return new HandStatistics(
      LowHandScore: lowHandScore,
      HighHandScore: highHandScore,
      AverageHandScore: totalHandScore / (double)numberOfHands,
      AverageCribScore: totalCribScore / (double)numberOfCribs
    );
  }

  public static List<(Hand, Card, int)> GetCribsAndScores(Card[] hand, Card[] discards)
  {
    var ret = new List<(Hand, Card, int)>();
    var remainingDeck = Utils.GetComplement([.. hand, .. discards]);

    for (int i = 0; i < remainingDeck.Length; i++)
    {
      var cutCard = remainingDeck[i];
      var rest = remainingDeck.Except([cutCard]).ToArray();

      foreach (var cribCards in Utils.NChooseC(rest, 2))
      {
        var cribHand = new Hand([.. discards, .. cribCards], IsCrib: true);
        var cribScore = ScoreCalculator.CalculateScore(CardCombinationEnumerator.GetCardCombinations(cribHand, cutCard));
        ret.Add((cribHand, cutCard, cribScore));
      }
    }

    return ret;
  }
}
