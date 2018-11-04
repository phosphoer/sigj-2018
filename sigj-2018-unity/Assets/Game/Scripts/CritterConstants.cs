using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CritterConstants
{
  public enum CreatureColor
  {
    White,
    Red,
    Blue,
    Orange,
    Green,
    Yellow,
    Purple
  }

  public static Color GetCreatureColorValue(CreatureColor color)
  {
    switch (color)
    {
      case CreatureColor.White:
        return Color.white;
      case CreatureColor.Red:
        return new Color(0.9f, 0.1f, 0.2f);
      case CreatureColor.Blue:
        return new Color(0.1f, 0.1f, 0.9f);
      case CreatureColor.Orange:
        return new Color(1.0f, 0.7f, 0.3f);
      case CreatureColor.Green:
        return new Color(0.1f, 0.9f, 0.1f);
      case CreatureColor.Yellow:
        return new Color(1.0f, 0.8f, 0.1f);
      case CreatureColor.Purple:
        return new Color(0.8f, 0.1f, 0.5f);
    }

    return Color.white;
  }

  public static string GetCreatureColorDisplayString(CreatureColor color)
  {
    switch (color)
    {
      case CreatureColor.White:
        return "White";
      case CreatureColor.Red:
        return "Red";
      case CreatureColor.Blue:
        return "Blue";
      case CreatureColor.Orange:
        return "Orange";
      case CreatureColor.Green:
        return "Green";
      case CreatureColor.Yellow:
        return "Yellow";
      case CreatureColor.Purple:
        return "Purple";
    }

    return "INVALID";
  }

  public enum CreatureShape
  {
    FloatingOrb,
    SlimeBlobGumdrop,
    CrawlingWorm,
    FloatingPeanut,
    LumpyCloud,
    PulsatingStarfish
  }

  public static string GetCreatureShapeDisplayString(CreatureShape Shape)
  {
    switch (Shape)
    {
      case CreatureShape.FloatingOrb:
        return "Floating Orb";
      case CreatureShape.SlimeBlobGumdrop:
        return "Slime Blob";
      case CreatureShape.CrawlingWorm:
        return "Crawling Worm";
      case CreatureShape.FloatingPeanut:
        return "Floating Peanut";
      case CreatureShape.LumpyCloud:
        return "Lumpy Cloud";
      case CreatureShape.PulsatingStarfish:
        return "Starfish";
    }

    return "INVALID";
  }

  public readonly static int CreatureSizeCount = System.Enum.GetValues(typeof(CreatureSize)).Length;
  public enum CreatureSize
  {
    Small,
    Medium,
    Large
  }

  public static string GetCreatureSizeDisplayString(CreatureSize Size)
  {
    switch (Size)
    {
      case CreatureSize.Small:
        return "Small";
      case CreatureSize.Medium:
        return "Medium";
      case CreatureSize.Large:
        return "Large";
    }

    return "INVALID";
  }

  public static float GetCreatureSizeScale(CreatureSize Size)
  {
    switch (Size)
    {
      case CreatureSize.Small:
        return 0.3f;
      case CreatureSize.Medium:
        return 0.4f;
      case CreatureSize.Large:
        return 0.6f;
    }

    return 1.0f;
  }

  public static float GetCreatureAgeAtSize(CreatureSize size)
  {
    switch (size)
    {
      case CreatureSize.Small:
        return 0.0f;
      case CreatureSize.Medium:
        return 0.33f;
      case CreatureSize.Large:
        return 0.66f;
    }

    return 0.0f;
  }

  public static CreatureSize GetCreatureSizeAtAge(float age)
  {
    if (age >= 0.66f)
      return CreatureSize.Large;
    else if (age >= 0.33f)
      return CreatureSize.Medium;

    return CreatureSize.Small;
  }

  public static T PickRandomEnum<T>()
  {
    Array values = Enum.GetValues(typeof(T));
    RangedInt enumRange = new RangedInt(0, values.Length);
    int randomIndex = enumRange.RandomValue;
    T randomEnumValue = (T)values.GetValue(randomIndex);

    return randomEnumValue;
  }

  public static CreatureColor PickRandomCreatureColor()
  {
    return PickRandomEnum<CreatureColor>();
  }

  public static CreatureShape PickRandomCreatureShape()
  {
    return PickRandomEnum<CreatureShape>();
  }

  public static CreatureSize PickRandomCreatureSize()
  {
    return PickRandomEnum<CreatureSize>();
  }
}
