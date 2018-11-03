using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : Singleton<CustomerOrderManager>
{
  public GameObject OrderPanelPrefab;
  public Transform PanelListTransform;
  public float PanelOffset= 13;
  public RangedFloat OrderTimeRange = new RangedFloat(5.0f, 10.0f);
  public int TotalOrders = 5;

  private int _OrdersIssued= 0;
  private IEnumerator _OrdersTimer;

  private List<GameObject> CustomerOrderPanelList = new List<GameObject>();

  // Use this for initialization
  void Start () {
    OnRoundStarted();
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void OnRoundStarted()
  {
    _OrdersIssued = 0;
    ClearOrders();
    StartOrderTimer();
  }

  public void OnRoundCompleted()
  {
    StopOrderTimer();
  }

  void StartOrderTimer()
  {
    StopOrderTimer();

    _OrdersTimer = IssueOrderTimer();
    StartCoroutine(_OrdersTimer);
  }

  void StopOrderTimer()
  {
    if (_OrdersTimer != null) {
      StopCoroutine(_OrdersTimer);
      _OrdersTimer = null;
    }
  }

  IEnumerator IssueOrderTimer()
  {
    while (_OrdersIssued < TotalOrders) {
      yield return new WaitForSeconds(OrderTimeRange.RandomValue);
      print("Adding a new random order");
      IssueRandomOrder();
    }

    print("Issued all orders for the round");
    yield return null;
  }

  public void IssueRandomOrder()
  {
    CustomerOrder newOrder = CreateRandomOrder();
    SpawnOrderPanel(newOrder);
    ++_OrdersIssued;
  }

  public void ClearOrders()
  {
    foreach (GameObject orderPanel in CustomerOrderPanelList) {
      Destroy(orderPanel);
    }
    CustomerOrderPanelList.Clear();
  }

  Vector3 GetNextPanelStartLocation()
  {
    Vector3 panelListRight = PanelListTransform.rotation * Vector3.right;

    return PanelListTransform.position + PanelOffset*panelListRight*(float)CustomerOrderPanelList.Count;
  }

  CustomerOrder CreateRandomOrder()
  {
    CustomerOrder newOrder = new CustomerOrder
    {
      OrderNumber = _OrdersIssued + 1,
      DesiredColor = CritterConstants.PickRandomCreatureColor(),
      DesiredShape = CritterConstants.PickRandomCreatureShape(),
      DesiredSize = CritterConstants.PickRandomCreatureSize()
    };

    return newOrder;
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
