using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class HandStatisticsCalculator
{
  public record HandStatistics(int LowHandScore, int HighHandScore, int LowCribScore, int HighCribScore, double AverageTotalScore);

  public static HandStatistics CalculateHandStatistics(Card[] hand, Card[] discards, bool ownsCrib)
  {
    var remainingDeck = Utils.GetComplement([.. hand, .. discards]);

    int totalHandScore = 0;
    int lowHandScore = int.MaxValue;
    int highHandScore = 0;
    int totalCribScore = 0;
    int lowCribScore = int.MaxValue;
    int highCribScore = 0;

    for (int i = 0; i < remainingDeck.Length; i++)
    {
      var cutCard = remainingDeck[i];
      var rest = remainingDeck.Except([cutCard]).ToArray();

      var handScore = ScoreCalculator.CalculateScore(CardCombinationEnumerator.GetCardCombinations(new Hand(hand, IsCrib: false), cutCard));
      if (handScore < lowHandScore) lowHandScore = handScore;
      if (handScore > highHandScore) highHandScore = handScore;
      totalHandScore += handScore;

      var allPossibleRemainingCribCards = Utils.NChooseC(rest, 2).ToArray();
      foreach (var cribCards in allPossibleRemainingCribCards)
      {
        var cribScore = ScoreCalculator.CalculateScore(CardCombinationEnumerator.GetCardCombinations(new Hand([.. discards, .. cribCards], IsCrib: true), cutCard));
        if (cribScore < lowCribScore) lowCribScore = cribScore;
        if (cribScore > highCribScore) highCribScore = cribScore;
        totalCribScore += cribScore;
      }
    }

    int numberOfHands = remainingDeck.Length;
    int numberOfCribs = numberOfHands * (remainingDeck.Length - 1) * (remainingDeck.Length - 2) / 2;

    Debug.WriteLine($"Number of hands: {numberOfHands}, Number of cribs: {numberOfCribs}");
    Debug.WriteLine($"Total hand score: {totalHandScore}, Total crib score: {totalCribScore}");

    return new HandStatistics(
      LowHandScore: lowHandScore,
      HighHandScore: highHandScore,
      LowCribScore: lowCribScore,
      HighCribScore: highCribScore,
      AverageTotalScore: totalHandScore / (double)numberOfHands + (ownsCrib ? 1 : -1) * totalCribScore / (double)numberOfCribs
    );
  }
}
