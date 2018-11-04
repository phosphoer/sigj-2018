using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerOrderPanel : MonoBehaviour, IPointerClickHandler
{

  private CustomerOrder _order;
  private TMPro.TextMeshPro _textMesh;
  private GameObject _highlight;
  private bool _bMarksInitialized = false;
  private bool _bIsShowingResults = false;

  public Material RedXMaterial;
  public Material GreenCheckMaterial;
  public GameObject[] ItemMarks;

  // Use this for initialization
  void Awake()
  {
    _textMesh = GetComponent<TMPro.TextMeshPro>();
  }

  void Start()
  {
    _highlight = this.transform.Find("HighlightPlane").gameObject;
    SetHighlightEnabled(false);

    // Hide all marks initially
    if (!_bMarksInitialized) {
      for (int itemMarkIndex = 0; itemMarkIndex < ItemMarks.Length; ++itemMarkIndex) {
        ItemMarks[itemMarkIndex].SetActive(false);
      }
    }
  }

  public void AssignCustomerOrder(CustomerOrder InOrder, bool bShowResults)
  {
    _order = InOrder;

    StringBuilder PanelStringBuilder = new StringBuilder();
    PanelStringBuilder.AppendLine(string.Format("Order: {0}", _order.OrderNumber));

    for (int itemMarkIndex = 0; itemMarkIndex < ItemMarks.Length; ++itemMarkIndex) {
      ItemMarks[itemMarkIndex].SetActive(false);
    }
    _bMarksInitialized = true;

    for (int desireIndex = 0; desireIndex < _order.CustomerDesires.Length; ++desireIndex)
    {
      PanelStringBuilder.AppendLine(_order.CustomerDesires[desireIndex].ToUIString());

      if (bShowResults) {        
        ItemMarks[desireIndex].SetActive(true);
        ItemMarks[desireIndex].GetComponent<MeshRenderer>().material = _order.CustomerDesires[desireIndex].DesireMet ? GreenCheckMaterial : RedXMaterial;
      }
    }

    _textMesh.SetText(PanelStringBuilder);
    _bIsShowingResults = bShowResults;
  }

  public CustomerOrder GetCustomerOrder()
  {
    return _order;
  }

  public void SetHighlightEnabled(bool bEnabled)
  {
    if (_highlight != null)
    {
      _highlight.SetActive(bEnabled);
    }
  }

  void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
  {
    if (!_bIsShowingResults) {
      CustomerOrderManager.Instance.OnOrderPanelClicked(this);
    }
  }
}
