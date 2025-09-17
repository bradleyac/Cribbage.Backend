using System;
using Cribbage.Core.Models;

namespace Cribbage.Core.Scoring;

public static class HandScoreCalculator
{
  public static int CalculateScore(IEnumerable<CardCombination> combinations) => combinations.Sum(c => c.CalculateScore());
}
