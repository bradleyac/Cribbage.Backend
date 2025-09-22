using System;
using System.Runtime.CompilerServices;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class CardCombinationEnumerator
{
  public static IEnumerable<CardCombination> GetCardCombinations(Hand hand, Card starter)
  {
    List<CardCombination> combinations = [];

    Card[] sortedAllCards = hand.Cards.Append(starter).OrderBy(card => card.Rank).ThenBy(card => card.Suit).ToArray();
    Card[][] sortedAllCombinations = GetAllCombinationsOfCards(sortedAllCards).ToArray();

    combinations.AddRange(GetPairs(sortedAllCards));
    combinations.AddRange(GetFifteens(sortedAllCombinations));
    combinations.AddRange(GetFlushes(hand, starter, sortedAllCards));
    combinations.AddRange(GetRuns(sortedAllCombinations));
    combinations.AddRange(GetNobs(hand, starter));

    return combinations;

    static IEnumerable<CardCombination> GetNobs(Hand hand, Card starter) => hand.Cards.FirstOrDefault(c => c.Rank == Rank.Jack && c.Suit == starter.Suit) is Card c
      ? [new CardCombination(CombinationType.Nobs, [c])]
      : [];

    static IEnumerable<CardCombination> GetRuns(Card[][] sortedAllCombinations) => sortedAllCombinations
            .Where(combo => combo.Length >= 3)
            .Where(Utils.IsRun)
            .GroupBy(c => c.Length)
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()?
            .Select(c => new CardCombination(CombinationType.Run, c)) ?? [];

    static IEnumerable<CardCombination> GetFifteens(Card[][] sortedAllCombinations) => sortedAllCombinations
            .GroupBy(c => c.Sum(card => Math.Min(10, (int)card.Rank)))
            .Where(g => g.Key == 15)
            .SelectMany(g => g.Select(combo => new CardCombination(CombinationType.Fifteen, combo)));

    static IEnumerable<CardCombination> GetFlushes(Hand hand, Card starter, Card[] sortedAllCards)
    {
      if (hand.Cards.All(c => c.Suit == hand.Cards[0].Suit))
      {
        if (starter.Suit == hand.Cards[0].Suit)
        {
          return [new CardCombination(CombinationType.Flush, sortedAllCards)];
        }
        else if (!hand.IsCrib)
        {
          return [new CardCombination(CombinationType.Flush, hand.Cards)];
        }
      }

      return [];
    }

    static IEnumerable<CardCombination> GetPairs(Card[] sortedAllCards)
    {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
      return sortedAllCards.GroupBy(c => c.Rank)
            .Where(g => g.Count() >= 2)
            .Select(g => new CardCombination(g.Count() switch { 2 => CombinationType.Pair, 3 => CombinationType.ThreeOfAKind, 4 => CombinationType.FourOfAKind }, g.ToArray()));
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
    }
  }

  private static IEnumerable<Card[]> GetAllCombinationsOfCards(Card[] cards) => GetAllCombinationsOfCards([], cards);

  private static IEnumerable<Card[]> GetAllCombinationsOfCards(Card[] acc, Card[] cards) => cards.Length switch
  {
    0 => [acc],
    _ => GetAllCombinationsOfCards([.. acc, cards[0]], cards[1..])
      .Concat(GetAllCombinationsOfCards(acc, cards[1..]))
  };
}
