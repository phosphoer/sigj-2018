using UnityEngine;

[System.Serializable]
public struct RangedFloat
{
  public float MinValue;
  public float MaxValue;

  public float RandomValue
  {
    get { return Random.Range(MinValue, MaxValue); }
  }

  public RangedFloat(float min = 0, float max = 0)
  {
    MinValue = min;
    MaxValue = max;
  }

  public bool InRange(float value, bool inclusive = true)
  {
    if (inclusive)
    {
      return value >= MinValue && value <= MaxValue;
    }

    return value > MinValue && value < MaxValue;
  }
}