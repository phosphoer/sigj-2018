using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderManager : Singleton<CustomerOrderManager>
{
  public GameObject OrderPanelPrefab;
  public Transform PanelListTransform;
  public Transform PanelListStartAnchor;
  public float PanelOffset = 13;
  public RangedFloat OrderTimeRange = new RangedFloat(5.0f, 10.0f);
  public RangedInt OrderDesiredChanges = new RangedInt(2, 2);
  public RangedInt DesiredAttachmentCount = new RangedInt(0, 5);
  public int TotalActiveOrders = 5;
  public AnimationCurve PanelDehydrateHeightCurve;

  private int _ordersIssued = 0;

  private IEnumerator _ordersTimer;
  private CustomerOrderPanel _selectedOrderPanel = null;
  private List<GameObject> _customerOrderPanelList = new List<GameObject>();
  private List<CustomerOrder> _completedOrdersList = new List<CustomerOrder>();
  private OutHatchController _outHatchController = null;
  private InHatchController _inHatchController = null;

  private void Awake()
  {
    CustomerOrderManager.Instance = this;
  }

  private void Update()
  {
    for (int i = 0; i < _customerOrderPanelList.Count; ++i)
    {
      Vector3 panelDesiredPos = GetPanelSlotLocation(i);
      GameObject panelObj = _customerOrderPanelList[i];
      panelObj.transform.position = Mathfx.Damp(panelObj.transform.position, panelDesiredPos, 0.5f, Time.deltaTime);
    }
  }

  public void OnRoundStarted()
  {
    _ordersIssued = 0;
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

    _ordersTimer = IssueOrderTimer();
    StartCoroutine(_ordersTimer);
  }

  void StopOrderTimer()
  {
    if (_ordersTimer != null)
    {
      StopCoroutine(_ordersTimer);
      _ordersTimer = null;
    }
  }

  IEnumerator IssueOrderTimer()
  {
    // StopOrderTimer() will kill this coroutine at the end of the round
    while (true)
    {
      yield return new WaitForSeconds(OrderTimeRange.RandomValue);

      if (_customerOrderPanelList.Count < TotalActiveOrders)
      {
        print("Adding a new random order");
        IssueRandomOrder();
      }
    }
  }

  public void IssueRandomOrder()
  {	
    // Create a new customer order
    CustomerOrder newOrder = CreateRandomOrder();

    // Notify the InHatch that a creature spawned
    _inHatchController.SpawnCreature(newOrder.SpawnDescriptor);

    // Spawn the order panel that shows what the customer wants
    SpawnOrderPanel(newOrder);

    // Keep track of how many orders we issues
    ++_ordersIssued;
  }

  public void ClearOrders()
  {
    foreach (GameObject orderPanel in _customerOrderPanelList)
    {
      Destroy(orderPanel);
    }
    _customerOrderPanelList.Clear();
  }

  Vector3 GetPanelSlotLocation(int slotIndex)
  {
    Vector3 panelListRight = PanelListTransform.rotation * Vector3.right;

    return PanelListTransform.position + PanelOffset * panelListRight * (float)slotIndex;
  }

  CustomerOrder CreateRandomOrder()
  {
    CustomerOrder newOrder = new CustomerOrder();
    newOrder.OrderNumber = _ordersIssued + 1;
    newOrder.SpawnDescriptor = CreatureDescriptor.CreateRandomCreatureDescriptor();

    int numDesiredChanges = OrderDesiredChanges.RandomValue;

    CustomerDesire.DesireType[] desiredChangeSequence = new CustomerDesire.DesireType[4] {
      CustomerDesire.DesireType.ChangeColor,
      CustomerDesire.DesireType.ChangeShape,
      CustomerDesire.DesireType.ChangeSize,
      CustomerDesire.DesireType.ChangeAttachments };
    ArrayUtilities.KnuthShuffle<CustomerDesire.DesireType>(desiredChangeSequence);

    newOrder.CustomerDesires = new CustomerDesire[numDesiredChanges];
    for (int desireIndex = 0; desireIndex < numDesiredChanges; ++desireIndex)
    {

      CustomerDesire.DesireType desiredChangeType = desiredChangeSequence[desireIndex];
      switch (desiredChangeType)
      {
        case CustomerDesire.DesireType.ChangeAttachments:
          {
            CreatureAttachmentDesire newDesire = new CreatureAttachmentDesire();
            newDesire.AttachmentDescriptor = CritterSpawner.Instance.PickRandomAttachmentDescriptor();
            newDesire.Count = DesiredAttachmentCount.RandomValue;

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
    // Create a panel at the initial location for them
    GameObject orderPanelObject = (GameObject)Instantiate(OrderPanelPrefab, PanelListStartAnchor.position, PanelListStartAnchor.rotation);
    CustomerOrderPanel orderPanelComponent = orderPanelObject.GetComponent<CustomerOrderPanel>();

    if (orderPanelComponent != null)
    {
      orderPanelComponent.AssignCustomerOrder(Order);
      _customerOrderPanelList.Add(orderPanelObject);
    }
  }

  void RebuildOrderPanelLayout()
  {
    for (int slotIndex = 0; slotIndex < _customerOrderPanelList.Count; ++slotIndex)
    {
      _customerOrderPanelList[slotIndex].transform.position = GetPanelSlotLocation(slotIndex);
    }
  }

  public void OnOrderPanelClicked(CustomerOrderPanel panel)
  {
    SetSelectedPanel(panel);
  }

  public void RegisterOutHatchController(OutHatchController controller)
  {
    _outHatchController = controller;
  }
  
  public void RegisterInHatchController(InHatchController controller)
  {
    _inHatchController = controller;
  }

  public void OnCreatureDeposited(CritterController critter)
  {
    bool bOrderSatisfied = false;

    if (_selectedOrderPanel != null)
    {
      // Get the order we are supposed to be fulfilling
      CustomerOrder order = _selectedOrderPanel.GetCustomerOrder();

      // Get the game object that owns the panel
      GameObject panelGameObject = _selectedOrderPanel.gameObject;

      // Remove the panel's GameObject from panel list
      _customerOrderPanelList.Remove(panelGameObject);

      // Deselect this panel 
      // - closes the out hatch
      // - invalidates _selectedOrderPanel
      SetSelectedPanel(null);

      // Apply the creature too the order
      bOrderSatisfied = order.TrySatisfyDesireWithCreatureDescriptor(critter.GetDNA());

      // Add the order to the completed order list
      _completedOrdersList.Add(order);

      // Destroy the panel
      StartCoroutine(DehydrateOrderPanelAsync(panelGameObject));
    }

    // Play the appropriate effect
    if (_outHatchController != null)
    {
      _outHatchController.OnCrittedScored(bOrderSatisfied);
    }

    // Destroy the creature deposited in the hatch
    Destroy(critter.gameObject);
  }
  
  private IEnumerator DehydrateOrderPanelAsync(GameObject panelObj)
  {
    float animDuration = 5.0f;
    Vector3 startPos = panelObj.transform.position;
    Vector3 endPos = panelObj.transform.position.WithZ(-10).WithY(0);
    for (float time = 0; time < animDuration; time += Time.deltaTime)
    {
      float t = Mathfx.Hermite(0, 1, time / animDuration);
      float heightT = PanelDehydrateHeightCurve.Evaluate(t);
      panelObj.transform.position = Vector3.Lerp(startPos, endPos, t).WithY(Mathf.Lerp(startPos.y, endPos.y, heightT));
      panelObj.transform.localRotation = Quaternion.Euler(heightT * 90, 0, 0);
      yield return null;
    }

    Destroy(panelObj);
  }

  private void SetSelectedPanel(CustomerOrderPanel panel)
  {
    if (panel != _selectedOrderPanel)
    {
      if (_selectedOrderPanel != null)
      {
        _selectedOrderPanel.SetHighlightEnabled(false);
      }

      if (panel != null)
      {
        panel.SetHighlightEnabled(true);
      }

      _selectedOrderPanel = panel;
    }
    else
    {
      if (_selectedOrderPanel != null)
      {
        _selectedOrderPanel.SetHighlightEnabled(false);
        _selectedOrderPanel = null;
      }
    }

    if (_outHatchController != null)
    {
      bool bOpenHatch = _selectedOrderPanel != null;
      _outHatchController.SetOpenState(bOpenHatch);
    }	
  }
}
