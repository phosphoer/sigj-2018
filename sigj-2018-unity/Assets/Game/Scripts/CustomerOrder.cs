using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerOrder {
  public int OrderNumber;
  public CritterConstants.CreatureColor DesiredColor;
  public CritterConstants.CreatureShape DesiredShape;
  public CritterConstants.CreatureSize DesiredSize;
}
