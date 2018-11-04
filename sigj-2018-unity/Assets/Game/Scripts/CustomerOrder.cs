using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerDesire
{
  public enum DesireType { Nothing, ChangeColor, ChangeShape, ChangeSize, ChangeAttachments };

  public virtual DesireType GetDesireType() {
    return DesireType.Nothing;
  }

  public virtual string ToUIString() {
    return "";
  }
}

[System.Serializable]
public class CustomerColorDesire : CustomerDesire
{
  public CritterConstants.CreatureColor DesiredColor = CritterConstants.CreatureColor.White;

  public override DesireType GetDesireType()
  {
    return DesireType.ChangeColor;
  }

  public override string ToUIString()
  {
    return string.Format("Color: {0}", CritterConstants.GetCreatureColorDisplayString(DesiredColor));
  }
}

[System.Serializable]
public class CustomerShapeDesire : CustomerDesire
{
  public CritterConstants.CreatureShape DesiredShape = CritterConstants.CreatureShape.FloatingOrb;

  public override DesireType GetDesireType()
  {
    return DesireType.ChangeShape;
  }

  public override string ToUIString()
  {
    return string.Format("Shape: {0}", CritterConstants.GetCreatureShapeDisplayString(DesiredShape));
  }
}

[System.Serializable]
public class CustomerSizeDesire : CustomerDesire
{
  public CritterConstants.CreatureSize DesiredSize = CritterConstants.CreatureSize.Large;

  public override DesireType GetDesireType()
  {
    return DesireType.ChangeSize;
  }

  public override string ToUIString()
  {
    return string.Format("Size: {0}", CritterConstants.GetCreatureSizeDisplayString(DesiredSize));
  }
}

[System.Serializable]
public class CreatureAttachmentDesire : CustomerDesire
{
  public CreatureAttachmentDescriptor AttachmentType;
  public int Count;

  public override DesireType GetDesireType()
  {
    return DesireType.ChangeAttachments;
  }

  public override string ToUIString()
  {
    if (Count > 1) {
      return string.Format("{0} {1}", Count, AttachmentType.UIPluralName);
    }
    else if (Count == 1) {
      return string.Format("1 {0}", AttachmentType.UISingularName);
    }
    else 
    {
      return string.Format("No {0}", AttachmentType.UIPluralName);
    }
  }
}

[System.Serializable]
public class CreatureDescriptor
{
  public CritterConstants.CreatureColor Color= CritterConstants.CreatureColor.White;
  public CritterConstants.CreatureShape Shape= CritterConstants.CreatureShape.FloatingOrb;
  public CritterConstants.CreatureSize Size= CritterConstants.CreatureSize.Small;
  public GameObject[] Attachments = new GameObject[0];

  public static CreatureDescriptor CreateRandomCreatureDescriptor()
  {
    CreatureDescriptor newDescriptor = new CreatureDescriptor();
    newDescriptor.Color = CritterConstants.PickRandomCreatureColor();
    newDescriptor.Shape = CritterConstants.PickRandomCreatureShape();
    newDescriptor.Size = CritterConstants.PickRandomCreatureSize();

    if (CritterSpawner.Instance != null) {
      newDescriptor.Attachments = CritterSpawner.Instance.PickNRandomAttachmentPrefabs();
    }

    return newDescriptor;
  }

  public static CreatureDescriptor CreateCreatureDescriptorFromParents(CritterController parent0, CritterController parent1)
  {
    CreatureDescriptor parentDNA0 = parent0.GetDNA();
    CreatureDescriptor parentDNA1 = parent1.GetDNA();
    CreatureDescriptor childDNA = new CreatureDescriptor();

    // Randomly pick a color from one of the parents
    childDNA.Color = Random.Range(0, 1) == 0 ? parentDNA0.Color : parentDNA1.Color;

    // Randomly pick a shape from one of the parents
    childDNA.Shape = Random.Range(0, 1) == 0 ? parentDNA0.Shape : parentDNA1.Shape;

    // Always start small
    childDNA.Size = CritterConstants.CreatureSize.Small; 

    // Combine the attachment from both parents into one big list
    List<GameObject> combinedParentAttachments = new List<GameObject>();
    combinedParentAttachments.AddRange(parentDNA0.Attachments);
    combinedParentAttachments.AddRange(parentDNA1.Attachments);

    // Create a shuffled index into the combined attachent list
    int[] shuffledParentAttachmentIndices= ArrayUtilities.MakeShuffledIntSequence(0, combinedParentAttachments.Count - 1);

    // Decide how many attachments the child will have
    //  1 -> SpawnAttachmentCount
    // but no more than parents had available to draw from
    int childAttachmentCount = System.Math.Min(System.Math.Max(CritterSpawner.Instance.SpawnAttachmentCount.RandomValue, 1), combinedParentAttachments.Count);

    // Fill in the child attachment list using the shuffled indices
    childDNA.Attachments = new GameObject[childAttachmentCount];
    for (int childAttachmentIndex= 0; childAttachmentIndex < childAttachmentCount; ++childAttachmentIndex) {
      int randomAttachmentIndex = shuffledParentAttachmentIndices[childAttachmentIndex];

      childDNA.Attachments[childAttachmentIndex] = combinedParentAttachments[randomAttachmentIndex];
    }

    return childDNA;
  }
}

[System.Serializable]
public class CustomerOrder {
  public int OrderNumber;
  public CreatureDescriptor SpawnDescriptor;
  public CustomerDesire[] CustomerDesires = new CustomerDesire[0];
}
