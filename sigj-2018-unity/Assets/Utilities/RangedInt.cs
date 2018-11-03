using UnityEngine;

[System.Serializable]
public struct RangedInt
{
  public int MinValue;
  public int MaxValue;

  public int RandomValue
  {
    get { return Random.Range(MinValue, MaxValue); }
  }

  public RangedInt(int min = 0, int max = 0)
  {
    MinValue = min;
    MaxValue = max;
  }

  public bool InRange(int value, bool inclusive = true)
  {
    if (inclusive)
    {
      return value >= MinValue && value <= MaxValue;
    }

    return value > MinValue && value < MaxValue;
  }

  public float DistanceFromRange(float value)
  {
    if (value <= MinValue)
    {
      return value - MinValue;
    }
    else if (value >= MaxValue)
    {
      return value - MaxValue;
    }

    return 0;
  }
}