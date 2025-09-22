using System;
using Cribbage.Core.Models;

namespace Cribbage.Core;

internal static class Utils
{
  public static List<T> Shuffle<T>(IList<T> list)
  {
    var shuffledList = list.ToList();
    ShuffleInPlace(shuffledList);
    return shuffledList;
  }

  public static void ShuffleInPlace<T>(IList<T> list)
  {
    int n = list.Count;
    while (n > 1)
    {
      int k = Random.Shared.Next(n--);
      (list[n], list[k]) = (list[k], list[n]);
    }
  }

  public static bool IsRun(Card[] combo)
  {
    combo = combo.OrderBy(c => c.Rank).ToArray();
    for (int i = 1; i < combo.Length; i++)
    {
      if ((int)combo[i].Rank != (int)combo[i - 1].Rank + 1)
      {
        return false;
      }
    }
    return true;
  }

  public static IEnumerable<Card[]> NChooseC(Card[] cards, int c)
  {
    if (c > cards.Length || c <= 0) yield break;
    if (c == cards.Length)
    {
      yield return cards;
      yield break;
    }
    if (c == 1)
    {
      foreach (var card in cards)
      {
        yield return new Card[] { card };
      }
      yield break;
    }

    for (int i = 0; i <= cards.Length - c; i++)
    {
      var head = cards[i];
      foreach (var tail in NChooseC(cards[(i + 1)..], c - 1))
      {
        yield return [head, .. tail];
      }
    }
  }

  public static Card[] GetComplement(Card[] cards) => Deck.DefaultCards.Except(cards).ToArray();
}
