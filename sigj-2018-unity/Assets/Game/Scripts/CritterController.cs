using UnityEngine;
using System.Collections;

public class CritterController : MonoBehaviour
{
  [SerializeField]
  private float _moveSpeed = 1;

  [SerializeField]
  private float _turnSpeed = 1;

  [SerializeField]
  private float _moveChance = 0.5f;

  // [SerializeField]
  // private float _vigorGainRate = 1.0f;

  [SerializeField]
  private RangedFloat _changeDirTimeRange = new RangedFloat(4, 8);

  [SerializeField]
  private Rigidbody _rigidBody = null;

  [SerializeField]
  private LayerMask _obstacleMask = Physics.DefaultRaycastLayers;

  [SerializeField]
  private float _obstacleAvoidDistance = 2;

  [SerializeField]
  private AnimationCurve _growUpCurve = null;

  private Vector3 _desiredDirection;
  private Vector3 _moveDirection;
  private float _changeDirectionTimer;
  private float _obstacleRaycastTimer;
  private bool _isMoving;
  private float _vigorLevel;
  private float _age;
  private CritterConstants.CreatureSize _size;

  private const float kAgeRate = 0.03f;
  private const float kGrowAnimationDuration = 1.0f;

  public void SetAge(float newAge, bool animate)
  {
    if (!animate)
    {
      _age = newAge;
      _size = CritterConstants.GetCreatureSizeAtAge(_age);
      transform.localScale = Vector3.one * CritterConstants.GetCreatureSizeScale(_size);
    }
    else
    {
      _age = newAge;
      SetSize(CritterConstants.GetCreatureSizeAtAge(_age));
    }
  }

  private void Start()
  {
    _desiredDirection = transform.forward;

    // Initialize size
    SetAge(_age, false);
  }

  private void FixedUpdate()
  {
    if (_isMoving)
    {
      _rigidBody.AddForce(_moveDirection.normalized * _moveSpeed, ForceMode.Force);
    }
  }

  private void Update()
  {
    // Age over time
    _age = Mathf.Clamp01(_age + Time.deltaTime * kAgeRate);
    CritterConstants.CreatureSize ageNewSize = CritterConstants.GetCreatureSizeAtAge(_age);
    if (ageNewSize != _size)
    {
      SetSize(ageNewSize);
    }

    // Change direction sometimes
    _changeDirectionTimer -= Time.deltaTime;
    if (_changeDirectionTimer < 0)
    {
      _changeDirectionTimer = _changeDirTimeRange.RandomValue;
      _desiredDirection = Random.onUnitSphere.WithY(0).normalized;
      _isMoving = Random.value > _moveChance;
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

  private void SetSize(CritterConstants.CreatureSize newSize)
  {
    _size = newSize;
    StartCoroutine(UpdateSizeAsync());
  }

  private IEnumerator UpdateSizeAsync()
  {
    Vector3 startScale = transform.localScale;
    Vector3 endScale = Vector3.one * CritterConstants.GetCreatureSizeScale(_size);
    for (float time = 0; time < kGrowAnimationDuration; time += Time.deltaTime)
    {
      float t = time / kGrowAnimationDuration;
      float tCurve = _growUpCurve.Evaluate(t);
      transform.localScale = Vector3.LerpUnclamped(startScale, endScale, tCurve);
      yield return null;
    }
  }
}