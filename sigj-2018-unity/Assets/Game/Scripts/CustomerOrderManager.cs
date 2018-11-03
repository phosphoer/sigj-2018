using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : Singleton<CustomerOrderManager>
{
  public GameObject OrderPanelPrefab;
  public Transform PanelListTransform;
  public float PanelOffset;

  private List<GameObject> CustomerOrderPanelList = new List<GameObject>();


  // Use this for initialization
  void Start () {
    CustomerOrder testOrder = new CustomerOrder
    {
      OrderNumber = 1,
      DesiredColor = CritterConstants.CreatureColor.Blue,
      DesiredShape = CritterConstants.CreatureShape.CrawlingWorm,
      DesiredSize = CritterConstants.CreatureSize.Small
    };

    SpawnOrderPanel(testOrder);

    CustomerOrder testOrder2 = new CustomerOrder
    {
      OrderNumber = 2,
      DesiredColor = CritterConstants.CreatureColor.Red,
      DesiredShape = CritterConstants.CreatureShape.FloatingOrb,
      DesiredSize = CritterConstants.CreatureSize.Large
    };

    SpawnOrderPanel(testOrder2);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  Vector3 GetNextPanelStartLocation()
  {
    Vector3 panelListRight = PanelListTransform.rotation * Vector3.right;

    return PanelListTransform.position + PanelOffset*panelListRight*(float)CustomerOrderPanelList.Count;
  }

  void SpawnOrderPanel(CustomerOrder Order)
  {
    // Instantiate the wreck game object at the same position we are at
    GameObject orderPanelObject = (GameObject)Instantiate(OrderPanelPrefab, GetNextPanelStartLocation(), PanelListTransform.rotation);
    CustomerOrderPanel orderPanelComponent= orderPanelObject.GetComponent<CustomerOrderPanel>();

    if (orderPanelComponent != null) {
      orderPanelComponent.AssignCustomerOrder(Order);

      CustomerOrderPanelList.Add(orderPanelObject);
    }
  }
}
