using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : Singleton<CustomerOrderManager>
{
  public GameObject OrderPanelPrefab;
  public Transform PanelListTransform;
  public float PanelOffset = 13;
  public RangedFloat OrderTimeRange = new RangedFloat(5.0f, 10.0f);
  public RangedInt OrderDesiredChanges = new RangedInt(2, 2);
  public RangedInt DesiredAttachmentCount = new RangedInt(0, 5);
  public int TotalOrders = 5;

  private int _OrdersIssued = 0;
  private IEnumerator _OrdersTimer;

  private List<GameObject> CustomerOrderPanelList = new List<GameObject>();

  private void Awake()
  {
    CustomerOrderManager.Instance = this;
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
    if (_OrdersTimer != null)
    {
      StopCoroutine(_OrdersTimer);
      _OrdersTimer = null;
    }
  }

  IEnumerator IssueOrderTimer()
  {
    while (_OrdersIssued < TotalOrders)
    {
      yield return new WaitForSeconds(OrderTimeRange.RandomValue);
      print("Adding a new random order");
      IssueRandomOrder();
    }

    print("Issued all orders for the round");
    yield return null;
  }

  public void IssueRandomOrder()
  {
    // Create a new customer order
    CustomerOrder newOrder = CreateRandomOrder();

    // Spawn a creature that corresponds to that order
    CritterSpawner.Instance?.SpawnCritter(newOrder.SpawnDescriptor);

    // Spawn the order panel that shows what the customer wants
    SpawnOrderPanel(newOrder);

    // Keep track of how many orders we issues
    ++_OrdersIssued;
  }

  public void ClearOrders()
  {
    foreach (GameObject orderPanel in CustomerOrderPanelList)
    {
      Destroy(orderPanel);
    }
    CustomerOrderPanelList.Clear();
  }

  Vector3 GetNextPanelStartLocation()
  {
    Vector3 panelListRight = PanelListTransform.rotation * Vector3.right;

    return PanelListTransform.position + PanelOffset * panelListRight * (float)CustomerOrderPanelList.Count;
  }

  CreatureDescriptor CreateRandomCreatureDescriptor()
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

  CustomerOrder CreateRandomOrder()
  {
    CustomerOrder newOrder = new CustomerOrder();
    newOrder.OrderNumber = _OrdersIssued + 1;
    newOrder.SpawnDescriptor = CreateRandomCreatureDescriptor();

    int numDesiredChanges = OrderDesiredChanges.RandomValue;

    CustomerDesire.DesireType[] desiredChangeSequence = new CustomerDesire.DesireType[4] {
      CustomerDesire.DesireType.ChangeColor,
      CustomerDesire.DesireType.ChangeShape,
      CustomerDesire.DesireType.ChangeSize,
      CustomerDesire.DesireType.ChangeAttachments };
    ArrayUtilities.KnuthShuffle<CustomerDesire.DesireType>(desiredChangeSequence);

    newOrder.CustomerDesires = new CustomerDesire[numDesiredChanges];
    for (int desireIndex= 0; desireIndex < numDesiredChanges; ++desireIndex) {

      CustomerDesire.DesireType desiredChangeType = desiredChangeSequence[desireIndex];
      switch (desiredChangeType) {
        case CustomerDesire.DesireType.ChangeAttachments: 
          {
            CreatureAttachmentDesire newDesire = new CreatureAttachmentDesire();
            newDesire.AttachmentType = CritterSpawner.Instance.PickRandomAttachmentDescriptor();
            newDesire.Count= DesiredAttachmentCount.RandomValue;

            newOrder.CustomerDesires[desireIndex] = newDesire;
          }
          break;
        case CustomerDesire.DesireType.ChangeColor: 
          {
            CustomerColorDesire newDesire = new CustomerColorDesire();
            newDesire.DesiredColor = CritterConstants.PickRandomCreatureColor();

            newOrder.CustomerDesires[desireIndex] = newDesire;
          }
          break;
        case CustomerDesire.DesireType.ChangeShape: 
          {
            CustomerShapeDesire newDesire = new CustomerShapeDesire();
            newDesire.DesiredShape = CritterConstants.PickRandomCreatureShape();

            newOrder.CustomerDesires[desireIndex] = newDesire;
          }
          break;
        case CustomerDesire.DesireType.ChangeSize: 
          {
            CustomerSizeDesire newDesire = new CustomerSizeDesire();
            newDesire.DesiredSize = CritterConstants.PickRandomCreatureSize();

            newOrder.CustomerDesires[desireIndex] = newDesire;
          }
          break;
      }
    }

    return newOrder;
  }

  void SpawnOrderPanel(CustomerOrder Order)
  {
    // Instantiate the wreck game object at the same position we are at
    GameObject orderPanelObject = (GameObject)Instantiate(OrderPanelPrefab, GetNextPanelStartLocation(), PanelListTransform.rotation);
    CustomerOrderPanel orderPanelComponent = orderPanelObject.GetComponent<CustomerOrderPanel>();

    if (orderPanelComponent != null)
    {
      orderPanelComponent.AssignCustomerOrder(Order);
      CustomerOrderPanelList.Add(orderPanelObject);
    }
  }
}
