using System;
using System.ComponentModel.DataAnnotations;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public class PeggingCardCombinationEnumerator
{
  public static IEnumerable<CardCombination> GetCardCombinations(Card[] cardSequence)
  {
    List<CardCombination> combinations = [];

    combinations.AddRange(GetPairs(cardSequence));
    combinations.AddRange(GetFifteenOrThirtyOne(cardSequence));
    combinations.AddRange(GetRuns(cardSequence));

    return combinations;

    static IEnumerable<CardCombination> GetPairs(Card[] cardSequence) => cardSequence.Reverse().TakeWhile(c => c.Rank == cardSequence[^1].Rank).Count() switch
    {
      2 => [new CardCombination(CombinationType.Pair, cardSequence[^2..])],
      3 => [new CardCombination(CombinationType.ThreeOfAKind, cardSequence[^3..])],
      4 => [new CardCombination(CombinationType.FourOfAKind, cardSequence[^4..])],
      _ => []
    };

    static IEnumerable<CardCombination> GetFifteenOrThirtyOne(Card[] cardSequence) => cardSequence.Sum(card => Math.Min(10, (int)card.Rank)) switch
    {
      15 => [new CardCombination(CombinationType.Fifteen, cardSequence)],
      31 => [new CardCombination(CombinationType.ThirtyOne, cardSequence)],
      _ => []
    };

    // Runs can't be longer than 7 cards in pegging because more than 7 would exceed 31 in all cases.
    // So only look at the first 7 cards maximum.
    static IEnumerable<CardCombination> GetRuns(Card[] cardSequence) => cardSequence.Length switch
    {
      < 3 => [],
      int length => Enumerable.Range(3, Math.Min(5, length - 2)).Select(runLength => cardSequence[^runLength..])
          .Where(Utils.IsRun)
          .Select(runCards => new CardCombination(CombinationType.Run, runCards))
          .TakeLast(1)
    };
  }
}
