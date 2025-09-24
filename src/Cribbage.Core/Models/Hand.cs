using System;

namespace Cribbage.Core.Models;

public record Hand(Card[] Cards, bool IsCrib) : IEquatable<Hand>
{
  public override string ToString() => $"{string.Join(",", Cards.Select(c => c.ToString()))}";

  public virtual bool Equals(Hand? other)
  {
    if (other is null) return false;
    if (IsCrib != other.IsCrib) return false;
    if (Cards.Length != other.Cards.Length) return false;

    var thisSorted = Cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank).ToArray();
    var otherSorted = other.Cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank).ToArray();

    return thisSorted.SequenceEqual(otherSorted);
  }

  public override int GetHashCode() => Cards.Aggregate(0, (acc, c) => HashCode.Combine(acc, c.GetHashCode()));
}