using System;

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
}
