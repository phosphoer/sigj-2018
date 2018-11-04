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

  public GameObject SpawnCritter(CreatureDescriptor SpawnDescriptor, Transform SpawnTransformOverride)
  {
    GameObject CritterPrefab = GetCritterPrefab(SpawnDescriptor.Shape);
    GameObject NewCritter = null;

    if (CritterPrefab != null) {
      Transform InitialTransform= SpawnTransformOverride != null ? SpawnTransformOverride : SpawnLocationTransform;

      NewCritter = Instantiate(CritterPrefab, InitialTransform.position, InitialTransform.rotation);

      CritterController ChildController= NewCritter.GetComponentInChildren<CritterController>();
      if (ChildController != null) {
        // Record the spawn descriptor (its "DNA") on the critter 
        ChildController.SetDNA(SpawnDescriptor);
      }

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

  public void PickNRandomAttachmentPrefabs(CreatureDescriptor newDescriptor)
  {
    int DescriptorCount = AttachmentDatabase.Length;

    if (DescriptorCount > 0) 
    {
      int AttachmentCount = SpawnAttachmentCount.RandomValue;
      newDescriptor.Attachments = new GameObject[AttachmentCount];
      newDescriptor.AttachmentTypes = new CreatureAttachmentDescriptor[AttachmentCount];

      for (int AttachmentIndex = 0; AttachmentIndex < AttachmentCount; ++AttachmentIndex) 
      {
        CreatureAttachmentDescriptor Descriptor = PickRandomAttachmentDescriptor();
        int VariationIndex = Random.Range(0, Descriptor.Variations.Length);

        newDescriptor.Attachments[AttachmentIndex] = Descriptor.Variations[VariationIndex];
        newDescriptor.AttachmentTypes[AttachmentIndex] = Descriptor;
      }
    }
    else
    {
      newDescriptor.Attachments= new GameObject[0];
      newDescriptor.AttachmentTypes = new CreatureAttachmentDescriptor[0];
    }
  }
}
