using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CustomerOrderPanel : MonoBehaviour {

  private CustomerOrder Order;
  private TMPro.TextMeshPro TextMesh;

	// Use this for initialization
	void Awake() {
    TextMesh = GetComponent<TMPro.TextMeshPro>();
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void AssignCustomerOrder(CustomerOrder InOrder)
  {
    Order = InOrder;

    StringBuilder PanelStringBuilder = new StringBuilder();
    PanelStringBuilder.AppendLine(string.Format("Order: {0}", Order.OrderNumber));
    PanelStringBuilder.AppendLine(string.Format("Shape: {0}", CritterConstants.GetCreatureShapeDisplayString(Order.DesiredShape)));
    PanelStringBuilder.AppendLine(string.Format("Color: {0}", CritterConstants.GetCreatureColorDisplayString(Order.DesiredColor)));
    PanelStringBuilder.AppendLine(string.Format("Size: {0}", CritterConstants.GetCreatureSizeDisplayString(Order.DesiredSize)));

    TextMesh.SetText(PanelStringBuilder);
  }
}
