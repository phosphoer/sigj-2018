using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{
  public GameObject CustomerOrderPrefab;
  public Transform PanelListStartAnchor;
  public float SpawnOrderDelay= 0.25f;
  public float PanelXOffset = 6.0f;
  public float PanelZOffset = -2.5f;

  private Queue<CustomerOrder> _pendingOrderSpawns = new Queue<CustomerOrder>();
  private List<GameObject> _customerOrderPanels = new List<GameObject>();

  // Use this for initialization
  void Start()
  {
    List<CustomerOrder> customerOrders= CustomerOrderManager.Instance.GetCompletedOrders();

    foreach(CustomerOrder customerOrder in customerOrders) {
      _pendingOrderSpawns.Enqueue(customerOrder);
    }

    StartCoroutine(SpawnOrderPanels());
  }

  private void OnDestroy()
  {
    for (int panelIndex= 0; panelIndex < _customerOrderPanels.Count; ++panelIndex) {
      Destroy(_customerOrderPanels[panelIndex]);
    }
    _customerOrderPanels.Clear();
  }

  private IEnumerator SpawnOrderPanels()
  {
    while (_pendingOrderSpawns.Count > 0) {
      yield return new WaitForSeconds(SpawnOrderDelay);

      CustomerOrder customerOrder = _pendingOrderSpawns.Dequeue();

      // Spawn a creature that corresponds to that order
      GameObject customerOrderObject = Instantiate(CustomerOrderPrefab, GetPanelSlotLocation(_customerOrderPanels.Count), PanelListStartAnchor.rotation);

      // Fill out the order form with check marks
      customerOrderObject.GetComponent<CustomerOrderPanel>().AssignCustomerOrder(customerOrder, true);

      // Remember the panels we spawned
      _customerOrderPanels.Add(customerOrderObject);
    }
  }

  Vector3 GetPanelSlotLocation(int slotIndex)
  {
    Vector3 panelListRight = PanelListStartAnchor.rotation * Vector3.right;
    Vector3 panelListForward = PanelListStartAnchor.rotation * Vector3.forward;

    return PanelListStartAnchor.position 
      + PanelXOffset * panelListRight * (float)slotIndex
      + PanelZOffset * panelListForward * (float)slotIndex;
  }

  public void OnMainMenuClicked()
  {
    GameStateManager.Instance.SetGameStage(GameStateManager.GameStage.MainMenu);
  }
}
