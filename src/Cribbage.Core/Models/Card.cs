namespace Cribbage.Core.Models;

public record Card(Suit Suit, Rank Rank)
{
  public override string ToString() => $"{(Rank > Rank.Ace && Rank < Rank.Jack ? ((int)Rank).ToString() : Rank.ToString().First())}{Suit.ToString().First()}";
}

public enum Suit
{
  Hearts,
  Diamonds,
  Clubs,
  Spades
}

public enum Rank
{
  Ace = 1,
  Two = 2,
  Three = 3,
  Four = 4,
  Five = 5,
  Six = 6,
  Seven = 7,
  Eight = 8,
  Nine = 9,
  Ten = 10,
  Jack = 11,
  Queen = 12,
  King = 13
}