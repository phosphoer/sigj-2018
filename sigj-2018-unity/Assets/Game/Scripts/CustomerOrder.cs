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
}

[System.Serializable]
public class CustomerOrder {
  public int OrderNumber;
  public CreatureDescriptor SpawnDescriptor;
  public CustomerDesire[] CustomerDesires = new CustomerDesire[0];
}
