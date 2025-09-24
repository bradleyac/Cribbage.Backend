using System;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;
using static Cribbage.Core.Scoring.HandStatisticsCalculator;

namespace Cribbage.Core.Tests.Scoring;

public class HandStatististicsCalculatorTests
{
  [Theory]
  // TODO: CribbagePro gives lower average crib scores for these hands.
  [InlineData("2H 3D JC QS", "6H 9C", 4, 11, 7.28, 5.79)]
  [InlineData("7S 8S 9H 10C", "4S 5H", 6, 14, 8.30, 7.35)]
  [InlineData("4S 7S 8S 9H", "5H 10C", 5, 12, 7.17, 7.18)]
  public void CalculateHandStatistics_ShouldReturnExpectedStatistics(string handString, string discardsString, int low, int high, double avgHand, double avgCrib)
  {
    Card[] hand = CreateCardsFromString(handString);
    Card[] discards = CreateCardsFromString(discardsString);

    var statistics = CalculateHandStatistics(hand, discards);

    Assert.Equal(new HandStatistics(low, high, avgHand, avgCrib), statistics);
  }

  [Theory]
  [InlineData("2H 3D JC QS", "6H 9C", 4, 11, 7.28, 5.79)]
  public void CalculateHandStatistics_ExportCribsAndScores(string handString, string discardsString, int low, int high, double avgHand, double avgCrib)
  {
    Card[] hand = CreateCardsFromString(handString);
    Card[] discards = CreateCardsFromString(discardsString);

    var statistics = GetCribsAndScores(hand, discards);

    foreach (var (crib, starter, score) in statistics)
    {
      Console.Error.WriteLine($"{starter},{string.Join(',', crib.Cards.Select(c => c.ToString()))}\t{score}");
    }
  }

  internal static Card[] CreateCardsFromString(string cardString)
  {
    return cardString.Split(' ', StringSplitOptions.RemoveEmptyEntries)
      .Select(s =>
      {
        var rankPart = s.Length == 3 ? s[..2] : s[0].ToString();
        var suitPart = s[^1];

        Rank rank = rankPart.ToUpper() switch
        {
          "A" => Rank.Ace,
          "2" => Rank.Two,
          "3" => Rank.Three,
          "4" => Rank.Four,
          "5" => Rank.Five,
          "6" => Rank.Six,
          "7" => Rank.Seven,
          "8" => Rank.Eight,
          "9" => Rank.Nine,
          "10" => Rank.Ten,
          "J" => Rank.Jack,
          "Q" => Rank.Queen,
          "K" => Rank.King,
          _ => throw new ArgumentException($"Invalid rank part: {rankPart}")
        };

        Suit suit = char.ToUpper(suitPart) switch
        {
          'H' => Suit.Hearts,
          'D' => Suit.Diamonds,
          'C' => Suit.Clubs,
          'S' => Suit.Spades,
          _ => throw new ArgumentException($"Invalid suit part: {suitPart}")
        };

        return new Card(suit, rank);
      })
      .ToArray();
  }
}
