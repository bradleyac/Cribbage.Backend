namespace Cribbage.Core.Models;

public class Deck
{
  internal static readonly Card[] DefaultCards = Enum.GetValues<Suit>()
      .SelectMany(suit => Enum.GetValues<Rank>().Select(rank => new Card(suit, rank)))
      .ToArray();

  private List<Card> _cards;
  public IReadOnlyList<Card> Cards => _cards;
  public Card StarterCard => _cards[^1];

  public Deck()
  {
    _cards = Utils.Shuffle(DefaultCards);
  }

  public void Shuffle()
  {
    Utils.ShuffleInPlace(_cards);
  }
}