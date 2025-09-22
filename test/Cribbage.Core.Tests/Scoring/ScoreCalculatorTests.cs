using System;
using Cribbage.Core.Models;
using Cribbage.Core.Scoring;
using static Cribbage.Core.Models.Suit;
using static Cribbage.Core.Models.Rank;

namespace Cribbage.Core.Tests.Scoring;

public class HandScoreCalculatorTests
{
  [Theory]
  [MemberData(nameof(HandsWithExpectedScores))]
  public void CalculateScore_ShouldScoreHandsCorrectly(Card[] handCards, Card starter, int expectedScore)
  {
    var hand = new Hand(handCards, false);
    var combinations = CardCombinationEnumerator.GetCardCombinations(hand, starter);
    var score = ScoreCalculator.CalculateScore(combinations);

    Assert.Equal(expectedScore, score);
  }

  [Theory]
  [MemberData(nameof(PeggingSequencesWithExpectedScores))]
  public void CalculateScore_ShouldScorePeggingSequencesCorrectly(Card[] cardSequence, int expectedScore)
  {
    var combinations = PeggingCardCombinationEnumerator.GetCardCombinations(cardSequence);
    var score = ScoreCalculator.CalculateScore(combinations);

    Assert.Equal(expectedScore, score);
  }

  public static IEnumerable<object[]> HandsWithExpectedScores = [
    [new Card[] { new(Clubs, Two), new(Clubs, Three), new(Clubs, Four), new(Clubs, Five) }, new Card(Clubs, Ace), 12], // Run of 5 (5), Flush (5), Fifteen (2)
    [new Card[] { new(Hearts, Five), new(Clubs, Five), new(Diamonds, Five), new(Spades, Jack) }, new Card(Spades, Five), 29], // Four of a Kind (12), 8 Fifteens (16) and his nobs (1),
    [new Card[] { new(Hearts, Three), new(Clubs, Three), new(Diamonds, Three), new(Spades, Jack) }, new Card(Spades, Three), 13], // Four of a Kind (12) and his nobs (1)
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Four) }, new Card(Clubs, Four), 17], // Triple run of 3 (9), Three of a kind (6), Fifteen (2)
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Five) }, new Card(Clubs, Six), 9], // Run of 5 (5), 2 Fifteens (4)
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Five) }, new Card(Clubs, Seven), 6], // Run of 4 (4), Fifteen (2)
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Six) }, new Card(Clubs, Seven), 7], // Run of 3 (3), 2 Fifteens (4)
    [new Card[] { new(Hearts, King), new(Clubs, King), new(Diamonds, King), new(Spades, King) }, new Card(Clubs, Five), 20], // Four of a kind (12), 4 Fifteens (8)
    [new Card[] { new(Hearts, King), new(Clubs, Queen), new(Diamonds, Jack), new(Spades, Ten) }, new Card(Clubs, Five), 12], // Run of four (4), 4 Fifteens (8)
    [new Card[] { new(Hearts, King), new(Clubs, King), new(Diamonds, Jack), new(Spades, Nine) }, new Card(Clubs, Five), 8], // 3 Fifteens (6), Pair (2),
    [new Card[] { new(Hearts, Ace), new(Clubs, Seven), new(Diamonds, Seven), new(Spades, Seven)}, new Card(Clubs, Five), 12], // Three of a kind (6), 3 Fifteens (6)
  ];

  public static IEnumerable<object[]> PeggingSequencesWithExpectedScores = [
    [new Card[] { new(Clubs, Two), new(Clubs, Three), new(Clubs, Four), new(Clubs, Five) }, 4], // Run of 4
    [new Card[] { new(Hearts, Five), new(Clubs, Five), new(Diamonds, Five), new(Spades, Jack) }, 0], // Nothing
    [new Card[] { new(Hearts, Three), new(Clubs, Three), new(Diamonds, Three), new(Spades, Jack) }, 0], // Nothing
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Four) }, 2], // Pair
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Five) }, 4], // Run of 4
    [new Card[] { new(Hearts, Two), new(Clubs, Three), new(Diamonds, Four), new(Spades, Six) }, 2], // Fifteen
    [new Card[] { new(Hearts, King), new(Clubs, King), new(Diamonds, King) }, 6], // Three of a kind
    [new Card[] { new(Hearts, King), new(Clubs, Queen), new(Diamonds, Jack) }, 3], // Run of 3
    [new Card[] { new(Hearts, King), new(Clubs, King), new(Diamonds, Jack) }, 0], // Nothing
    [new Card[] { new(Hearts, Ace), new(Clubs, Seven), new(Diamonds, Seven), new(Spades, Seven)}, 6], // Three of a kind
    [new Card[] { new(Hearts, Five), new(Clubs, Five), new(Spades, Five) }, 8], // Fifteen + Three of a kind
    [new Card[] { new(Hearts, Five), new(Clubs, Five), new(Spades, Five), new(Diamonds, Five), new(Diamonds, Jack), new(Diamonds, Ace) }, 2], // 31
    [new Card[] { new(Hearts, Ace), new(Clubs, Seven), new(Diamonds, Three), new(Spades, Four), new(Hearts, Five), new(Clubs, Six), new(Clubs, Two) }, 7], // Run of 7
  ];
}