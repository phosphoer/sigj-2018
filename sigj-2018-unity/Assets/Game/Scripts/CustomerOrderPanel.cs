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

	// Use this for initialization
	void Awake() {
    _textMesh = GetComponent<TMPro.TextMeshPro>();
  }

  void Start()
  {
    _highlight = this.transform.Find("HighlightPlane").gameObject;
    SetHighlightEnabled(false);
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  public void AssignCustomerOrder(CustomerOrder InOrder)
  {
    _order = InOrder;

    StringBuilder PanelStringBuilder = new StringBuilder();
    PanelStringBuilder.AppendLine(string.Format("Order: {0}", _order.OrderNumber));

    for (int desireIndex= 0; desireIndex < _order.CustomerDesires.Length; ++desireIndex) {
      PanelStringBuilder.AppendLine(_order.CustomerDesires[desireIndex].ToUIString());
    }

    _textMesh.SetText(PanelStringBuilder);
  }

  public void SetHighlightEnabled(bool bEnabled)
  {
    if (_highlight != null) {
      _highlight.SetActive(bEnabled);
    }
  }

  void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
  {
    CustomerOrderManager.Instance.OnOrderPanelClicked(this);
  }
}
