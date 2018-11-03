using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : Singleton<CritterSpawner>
{
  public Transform SpawnLocationTransform;

  public GameObject CritterCloudPrefab;
  public GameObject CritterPeanutPrefab;
  public GameObject CritterSlimePrefab;
  public GameObject CritterSpherePrefab;
  public GameObject CritterStarPrefab;
  public GameObject CritterWormPrefab;

  public void Awake()
  {
    CritterSpawner.Instance = this;
  }

  public GameObject SpawnCritter(CustomerOrder order)
  {
    CritterConstants.CreatureShape Shape = CritterConstants.PickRandomCreatureShape();
    GameObject CritterPrefab = GetCritterPrefab(Shape);
    GameObject NewCritter = null;

    if (CritterPrefab != null) {
      NewCritter = Instantiate(CritterPrefab, SpawnLocationTransform.position, SpawnLocationTransform.rotation);
    }

    return NewCritter;
  }

  private GameObject GetCritterPrefab(CritterConstants.CreatureShape Shape)
  {
    switch (Shape) {
      case CritterConstants.CreatureShape.FloatingOrb:
        return CritterSpherePrefab;
      case CritterConstants.CreatureShape.SlimeBlobGumdrop:
        return CritterSlimePrefab;
      case CritterConstants.CreatureShape.CrawlingWorm:
        return CritterWormPrefab;
      case CritterConstants.CreatureShape.FloatingPeanut:
        return CritterPeanutPrefab;
      case CritterConstants.CreatureShape.LumpyCloud:
        return CritterCloudPrefab;
      case CritterConstants.CreatureShape.PulsatingStarfish:
        return CritterStarPrefab;
    }

    return null;
  }
}
