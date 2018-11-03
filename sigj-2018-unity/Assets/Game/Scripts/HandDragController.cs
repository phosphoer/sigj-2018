using UnityEngine;

public class HandDragController : MonoBehaviour
{
  public bool IsDragging => _currentDraggable != null;

  [SerializeField]
  private Transform _handObject = null;

  [SerializeField]
  private LayerMask _raycastMask = Physics.DefaultRaycastLayers;

  [SerializeField]
  private float _handHoverDistance = 1.0f;

  [SerializeField]
  private float _handDragHeight = 3.0f;

  [SerializeField]
  private float _handDragSensitivity = 3.0f;

  [SerializeField]
  private float _handAnimateSpeed = 6.0f;

  private Vector3 _targetHandPos;
  private Rigidbody _currentDraggable;
  private float _initialDragValue;

  private void FixedUpdate()
  {
    if (_currentDraggable != null)
    {
      Vector3 toHand = _targetHandPos - _currentDraggable.position;
      _currentDraggable.AddForce(toHand * 100);
    }
  }

  private void Update()
  {
    if (IsDragging)
    {
      float distanceScale = Vector3.Distance(Camera.main.transform.position, _currentDraggable.position);
      float horizontalAxis = Input.GetAxis("Mouse X") * Time.deltaTime * _handDragSensitivity;
      float verticalAxis = Input.GetAxis("Mouse Y") * Time.deltaTime * _handDragSensitivity;
      _targetHandPos += Camera.main.transform.right.WithY(0).normalized * horizontalAxis * distanceScale;
      _targetHandPos += Camera.main.transform.forward.WithY(0).normalized * verticalAxis * distanceScale;
      _targetHandPos.y = _handDragHeight;
    }
    else
    {
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      bool hit = Physics.Raycast(mouseRay, out hitInfo, 100.0f, _raycastMask);
      if (hit)
      {
        _targetHandPos = hitInfo.point + hitInfo.normal * _handHoverDistance;

        if (Input.GetMouseButtonDown(0))
        {
          StartGrab(hitInfo);
        }
      }
    }

    if (Input.GetMouseButtonUp(0))
    {
      StopGrab();
    }

    _handObject.transform.position = Mathfx.Damp(_handObject.transform.position, _targetHandPos, 0.5f, Time.deltaTime * _handAnimateSpeed);
  }

  private void StartGrab(RaycastHit forHitInfo)
  {
    if (forHitInfo.rigidbody != null && !forHitInfo.rigidbody.isKinematic)
    {
      _currentDraggable = forHitInfo.rigidbody;
      _initialDragValue = _currentDraggable.drag;
      _currentDraggable.drag = 10;
      _handObject.gameObject.SetActive(false);
    }
  }

  private void StopGrab()
  {
    if (_currentDraggable != null)
    {
      _currentDraggable.drag = _initialDragValue;
      _currentDraggable = null;
      _handObject.gameObject.SetActive(true);
    }
  }
}