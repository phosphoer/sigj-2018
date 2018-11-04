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

    for (int desireIndex= 0; desireIndex < Order.CustomerDesires.Length; ++desireIndex) {
      PanelStringBuilder.AppendLine(Order.CustomerDesires[desireIndex].ToUIString());
    }

    TextMesh.SetText(PanelStringBuilder);
  }
}
