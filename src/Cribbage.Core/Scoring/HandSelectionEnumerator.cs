using System;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class HandSelectionEnumerator
{
  public static IEnumerable<(Card[] hand, Card[] discards)> GetHandSelections(Card[] initialHand)
  {
    foreach (var discards in Utils.NChooseC(initialHand, 2))
    {
      var handSelection = initialHand.Except(discards).ToArray();
      yield return (handSelection, discards);
    }
  }
}
