using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : Singleton<CritterSpawner>
{
  public Transform SpawnLocationTransform;

  public RangedInt SpawnAttachmentCount = new RangedInt(0, 5);

  public GameObject CritterCloudPrefab;
  public GameObject CritterPeanutPrefab;
  public GameObject CritterSlimePrefab;
  public GameObject CritterSpherePrefab;
  public GameObject CritterStarPrefab;
  public GameObject CritterWormPrefab;

  public CreatureAttachmentDescriptor[] AttachmentDatabase = new CreatureAttachmentDescriptor[0];

  public void Awake()
  {
    CritterSpawner.Instance = this;
  }

  public GameObject SpawnCritter(CreatureDescriptor SpawnDescriptor)
  {
    GameObject CritterPrefab = GetCritterPrefab(SpawnDescriptor.Shape);
    GameObject NewCritter = null;

    if (CritterPrefab != null) {
      NewCritter = Instantiate(CritterPrefab, SpawnLocationTransform.position, SpawnLocationTransform.rotation);

      // Spawn attachments on the critter
      CritterAttachmentManager AttachmentManager= NewCritter.GetComponentInChildren<CritterAttachmentManager>();
      if (AttachmentManager != null) {
        AttachmentManager.SpawnAttachments(SpawnDescriptor);
      }
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

  public CreatureAttachmentDescriptor PickRandomAttachmentDescriptor()
  {
    int DescriptorCount = AttachmentDatabase.Length;

    if (DescriptorCount > 0) {
      int DescriptorIndex = Random.Range(0, DescriptorCount);
      return AttachmentDatabase[DescriptorIndex];
    }
    else {
      return null;
    }
  }

  public GameObject[] PickNRandomAttachmentPrefabs()
  {
    int DescriptorCount = AttachmentDatabase.Length;

    if (DescriptorCount > 0) 
    {
      int AttachmentCount = SpawnAttachmentCount.RandomValue;
      GameObject[] Attachments = new GameObject[AttachmentCount];

      for (int AttachmentIndex = 0; AttachmentIndex < AttachmentCount; ++AttachmentIndex) 
      {
        CreatureAttachmentDescriptor Descriptor = PickRandomAttachmentDescriptor();
        int VariationIndex = Random.Range(0, Descriptor.Variations.Length);

        Attachments[AttachmentIndex] = Descriptor.Variations[VariationIndex];
      }

      return Attachments;
    }
    else
    {
      return new GameObject[0];
    }
  }
}
