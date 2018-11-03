using UnityEngine;

public class CritterController : MonoBehaviour
{
  [SerializeField]
  private float _moveSpeed = 1;

  [SerializeField]
  private float _turnSpeed = 1;

  [SerializeField]
  private RangedFloat _changeDirTimeRange = new RangedFloat(4, 8);

  [SerializeField]
  private Rigidbody _rigidBody = null;

  [SerializeField]
  private LayerMask _obstacleMask = Physics.DefaultRaycastLayers;

  [SerializeField]
  private float _obstacleAvoidDistance = 2;

  private Vector3 _desiredDirection;
  private Vector3 _moveDirection;
  private float _changeDirectionTimer;
  private float _obstacleRaycastTimer;

  private void Start()
  {
    _desiredDirection = transform.forward;
  }

  private void FixedUpdate()
  {
    _rigidBody.AddForce(_moveDirection.normalized * _moveSpeed, ForceMode.Force);
  }

  private void Update()
  {
    // Change direction sometimes
    _changeDirectionTimer -= Time.deltaTime;
    if (_changeDirectionTimer < 0)
    {
      _changeDirectionTimer = _changeDirTimeRange.RandomValue;
      _desiredDirection = Random.onUnitSphere.WithY(0).normalized;
    }

    // Slowly change move direction towards current desired direction / rotate to face move direction
    _moveDirection = Mathfx.Damp(_moveDirection, _desiredDirection, 0.5f, Time.deltaTime * _turnSpeed);

    Quaternion desiredRot = Quaternion.LookRotation(_moveDirection, Vector3.up);
    _rigidBody.rotation = Mathfx.Damp(_rigidBody.rotation, desiredRot, 0.5f, Time.deltaTime * _turnSpeed);

    // Occaisonally raycast for obstacles
    _obstacleRaycastTimer -= Time.deltaTime;
    if (_obstacleRaycastTimer < 0)
    {
      _obstacleRaycastTimer = 2;

      RaycastHit hitInfo;
      if (Physics.Raycast(transform.position, _desiredDirection, out hitInfo, _obstacleAvoidDistance, _obstacleMask))
      {
        _desiredDirection = Quaternion.Euler(0, 90, 0) * _desiredDirection;
        _changeDirectionTimer = _changeDirTimeRange.MaxValue;
        Debug.DrawLine(transform.position, hitInfo.point, Color.red, 1.0f);
      }
      else
      {
        Debug.DrawRay(transform.position, _desiredDirection * _obstacleAvoidDistance, Color.white, 0.5f);
      }
    }

    Debug.DrawRay(transform.position, _desiredDirection, Color.blue);
  }
}