using System;
using System.Runtime.CompilerServices;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class CardCombinationEnumerator
{
  public static IEnumerable<CardCombination> GetCardCombinations(Hand hand, Card starter)
  {
    List<CardCombination> combinations = new();

    Card[] sortedAllCards = hand.Cards.Append(starter).OrderBy(card => card.Rank).ThenBy(card => card.Suit).ToArray();

    combinations.AddRange(GetPairs(sortedAllCards));
    combinations.AddRange(GetFifteens(sortedAllCards));
    combinations.AddRange(GetFlushes(hand, starter, sortedAllCards));
    combinations.AddRange(GetRuns(sortedAllCards));
    combinations.AddRange(GetNobs(hand, starter));

    return combinations;

    static IEnumerable<CardCombination> GetNobs(Hand hand, Card starter)
    {
      return hand.Cards.FirstOrDefault(c => c.Rank == Rank.Jack && c.Suit == starter.Suit) is Card c ? [new CardCombination(CombinationType.Nobs, [c])] : [];
    }

    static IEnumerable<CardCombination> GetRuns(Card[] sortedAllCards)
    {
      return GetAllCombinationsOfCards(sortedAllCards)
            .Where(combo => combo.Length >= 3)
            .Where(IsRun)
            .GroupBy(c => c.Length)
            .OrderByDescending(g => g.Key)
            .FirstOrDefault()?
            .Select(c => new CardCombination(CombinationType.Run, c)) ?? [];

      static bool IsRun(Card[] combo)
      {
        for (int i = 1; i < combo.Length; i++)
        {
          if ((int)combo[i].Rank != (int)combo[i - 1].Rank + 1)
          {
            return false;
          }
        }
        return true;
      }
    }

    static IEnumerable<CardCombination> GetFifteens(Card[] allCards)
    {
      return GetAllCombinationsOfCards(allCards)
            .GroupBy(c => c.Sum(card => Math.Min(10, (int)card.Rank)))
            .Where(g => g.Key == 15)
            .SelectMany(g => g.Select(combo => new CardCombination(CombinationType.Fifteen, combo)));
    }

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

  private static IEnumerable<Card[]> GetAllCombinationsOfCards(Card[] cards)
  {
    int n = cards.Length;
    int combinationCount = 1 << n; // 2^n combinations

    for (int i = 1; i < combinationCount; i++)
    {
      List<Card> combination = new();
      for (int j = 0; j < n; j++)
      {
        if ((i & (1 << j)) != 0)
        {
          combination.Add(cards[j]);
        }
      }
      yield return combination.ToArray();
    }
  }
}
