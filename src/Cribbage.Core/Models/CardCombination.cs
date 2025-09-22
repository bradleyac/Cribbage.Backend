using System;

namespace Cribbage.Core.Models;

public sealed record CardCombination(CombinationType Type, Card[] Cards) : IEquatable<CardCombination>
{
  public int CalculateScore() => Type switch
  {
    CombinationType.Fifteen => 2,
    CombinationType.ThirtyOne => 2,
    CombinationType.Pair => 2,
    CombinationType.ThreeOfAKind => 6,
    CombinationType.FourOfAKind => 12,
    CombinationType.Run => Cards.Length,
    CombinationType.Flush => Cards.Length,
    CombinationType.Nobs => 1,
    _ => 0
  };

  public bool Equals(CardCombination? other)
  {
    if (other is null) return false;
    if (Type != other.Type) return false;
    if (Cards.Length != other.Cards.Length) return false;

    var thisSorted = Cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank).ToArray();
    var otherSorted = other.Cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank).ToArray();

    return thisSorted.SequenceEqual(otherSorted);
  }

  public override int GetHashCode()
  {
    int hash = Type.GetHashCode();
    foreach (var card in Cards.OrderBy(c => c.Suit).ThenBy(c => c.Rank))
    {
      hash = HashCode.Combine(hash, card);
    }
    return hash;
  }
}

public enum CombinationType
{
  Pair,
  ThreeOfAKind,
  FourOfAKind,
  Run,
  Fifteen,
  ThirtyOne,
  Flush,
  Nobs,
}