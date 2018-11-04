using UnityEngine;

public class HandDragController : MonoBehaviour
{
  public static event System.Action<GameObject> DragStart;
  public static event System.Action DragStop;

  public bool IsDragging { get; private set; }

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
  private Vector3 _dragStartCameraPos;

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
    if (!Rewired.ReInput.isReady)
    {
      return;
    }

    Rewired.Player player = Rewired.ReInput.players.GetPlayer(0);

    if (IsDragging)
    {
      // If current dragged thing was destroyed, cancel drag
      if (_currentDraggable == null)
      {
        StopDrag();
        return;
      }

      float distanceScale = Vector3.Distance(Camera.main.transform.position, _currentDraggable.position);
      float horizontalAxis = player.GetAxis("CursorX") * Time.deltaTime * _handDragSensitivity;
      float verticalAxis = player.GetAxis("CursorY") * Time.deltaTime * _handDragSensitivity;
      Vector3 cameraMovement = Camera.main.transform.position - _dragStartCameraPos;
      _targetHandPos += Camera.main.transform.right.WithY(0).normalized * horizontalAxis * distanceScale;
      _targetHandPos += Camera.main.transform.forward.WithY(0).normalized * verticalAxis * distanceScale;
      _targetHandPos += cameraMovement;
      _targetHandPos.y = _handDragHeight;
      _dragStartCameraPos = Camera.main.transform.position;
    }
    else
    {
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      bool hit = Physics.Raycast(mouseRay, out hitInfo, 100.0f, _raycastMask);
      if (hit)
      {
        _targetHandPos = hitInfo.point + hitInfo.normal * _handHoverDistance;

        if (player.GetButtonDown("Select"))
        {
          StartDrag(hitInfo);
        }
      }
    }

    if (player.GetButtonUp("Select"))
    {
      StopDrag();
    }

    _handObject.transform.position = Mathfx.Damp(_handObject.transform.position, _targetHandPos, 0.5f, Time.deltaTime * _handAnimateSpeed);
  }

  private void StartDrag(RaycastHit forHitInfo)
  {
    if (forHitInfo.rigidbody != null && !forHitInfo.rigidbody.isKinematic)
    {
      IsDragging = true;

      _currentDraggable = forHitInfo.rigidbody;
      _initialDragValue = _currentDraggable.drag;
      _currentDraggable.drag = 10;
      _dragStartCameraPos = Camera.main.transform.position;

      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Confined;

      DragStart?.Invoke(_currentDraggable.gameObject);
    }
  }

  private void StopDrag()
  {
    if (_currentDraggable != null)
    {
      _currentDraggable.drag = _initialDragValue;
    }

    IsDragging = false;
    _currentDraggable = null;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;

    DragStop?.Invoke();
  }
}