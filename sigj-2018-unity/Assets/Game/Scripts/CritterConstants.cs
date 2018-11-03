using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CritterConstants {
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

  public static string GetCreatureColorDisplayString(CreatureColor Color)
  {
    switch(Color) {
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
    switch (Shape) {
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

  public enum CreatureSize
  {
    Small,
    Medium,
    Large
  }

  public static string GetCreatureSizeDisplayString(CreatureSize Size)
  {
    switch (Size) {
      case CreatureSize.Small:
        return "Small";
      case CreatureSize.Medium:
        return "Medium";
      case CreatureSize.Large:
        return "Large";
    }

    return "INVALID";
  }
}
