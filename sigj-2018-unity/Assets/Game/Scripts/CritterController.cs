using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CritterController : MonoBehaviour
{
  public bool IsReadyForLove => _vigorLevel >= 1 && !IsMating;
  public bool IsMating => _currentMate != null;

  [SerializeField]
  private float _moveSpeed = 1;

  [SerializeField]
  private float _turnSpeed = 1;

  [SerializeField]
  private float _moveChance = 0.5f;

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

  [SerializeField]
  private EggController _eggPrefab = null;

  [SerializeField]
  private GameObject _mateEffectPrefab = null;

  [SerializeField]
  private ProgressBarUI _vigorUIPrefab = null;

  [SerializeField]
  private Transform _vigorUIRoot = null;

  [SerializeField]
  private Transform _visualRoot = null;

  [SerializeField]
  private Animator _animator = null;

  private Vector3 _desiredDirection;
  private Vector3 _moveDirection;
  private float _changeDirectionTimer;
  private float _obstacleRaycastTimer;
  private bool _isMoving;
  private float _vigorLevel;
  private float _age;
  private CritterConstants.CreatureSize _size;
  private ProgressBarUI _vigorUI;
  private int _mateSearchIndex;
  private CritterController _nearestMate;
  private CritterController _currentMate;
  private CreatureDescriptor _critterDNA;
  private Material _critterMaterialInstance;

  private const float kAgeRate = 0.02f;
  private const float kGrowAnimationDuration = 1.0f;
  private const float kMinMateDistance = 2.0f;
  private const float kVigorGainRate = 0.02f;
  private const CritterConstants.CreatureSize kMinVigorSize = CritterConstants.CreatureSize.Medium;

  private static readonly int kAnimParamIsWalking = Animator.StringToHash("IsWalking");
  private static List<CritterController> _instances = new List<CritterController>();

  public void SetDNA(CreatureDescriptor DNA)
  {
    _critterDNA = DNA;

    // Spawn attachments on the critter
    CritterAttachmentManager AttachmentManager = GetComponentInChildren<CritterAttachmentManager>();
    if (AttachmentManager != null)
    {
      AttachmentManager.SpawnAttachments(_critterDNA);
    }

    // Apply color settings
    HasCritterColor[] childColors = _visualRoot.GetComponentsInChildren<HasCritterColor>();
    foreach (var childColor in childColors)
    {
      var r = childColor.GetComponent<Renderer>();
      if (r != null)
      {
        if (_critterMaterialInstance == null)
          _critterMaterialInstance = Instantiate(r.sharedMaterial);

        _critterMaterialInstance.color = CritterConstants.GetCreatureColorValue(_critterDNA.Color);
        r.sharedMaterial = _critterMaterialInstance;
      }
    }

    SetAge(CritterConstants.GetCreatureAgeAtSize(_critterDNA.Size), animate: false);
  }

  public CreatureDescriptor GetDNA()
  {
    return _critterDNA;
  }

  public void SetAge(float newAge, bool animate)
  {
    _age = newAge;
    SetSize(CritterConstants.GetCreatureSizeAtAge(_age), animate);
  }

  private void Awake()
  {
    _vigorUI = Instantiate(_vigorUIPrefab, _vigorUIRoot);
    _vigorUI.transform.localPosition = Vector3.zero;
    _vigorUI.transform.localScale = Vector3.one;
    _vigorUI.gameObject.SetActive(false);
  }

  private void Start()
  {
    _instances.Add(this);

    _desiredDirection = transform.forward;

    // Initialize size
    SetAge(_age, animate: false);
  }

  private void OnDestroy()
  {
    _instances.Remove(this);
    Destroy(_critterMaterialInstance);
  }

  private void FixedUpdate()
  {
    if (_isMoving)
    {
      float moveSpeed = _moveSpeed * _visualRoot.localScale.x;
      _rigidBody.AddForce(_moveDirection.normalized * moveSpeed, ForceMode.Force);
    }
  }

  private void Update()
  {
    // Age over time
    _age = Mathf.Clamp01(_age + Time.deltaTime * kAgeRate);
    CritterConstants.CreatureSize ageNewSize = CritterConstants.GetCreatureSizeAtAge(_age);
    if (ageNewSize != _size)
    {
      SetSize(ageNewSize, animate: true);
    }

    // Gain vigor, if we're old enough
    if ((int)_size >= (int)kMinVigorSize)
    {
      _vigorLevel = Mathf.Clamp01(_vigorLevel + Time.deltaTime * kVigorGainRate);
      _vigorUI.FillPercent = _vigorLevel;
    }

    // Change direction sometimes
    _changeDirectionTimer -= Time.deltaTime;
    if (_changeDirectionTimer < 0)
    {
      _changeDirectionTimer = _changeDirTimeRange.RandomValue;
      _desiredDirection = Random.onUnitSphere.WithY(0).normalized;
      _isMoving = Random.value > _moveChance;

      if (_animator != null)
      {
        _animator.SetBool(kAnimParamIsWalking, _isMoving);
      }
    }

    // Slowly change move direction towards current desired direction / rotate to face move direction
    _moveDirection = Mathfx.Damp(_moveDirection, _desiredDirection, 0.5f, Time.deltaTime * _turnSpeed).WithY(0);

    if (_moveDirection.magnitude > 0.1f)
    {
      Quaternion desiredRot = Quaternion.LookRotation(_moveDirection, Vector3.up);
      _rigidBody.rotation = Mathfx.Damp(_rigidBody.rotation, desiredRot, 0.5f, Time.deltaTime * _turnSpeed);
    }

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

    // If we have full vigor, look for the closest mate and go get em
    if (IsReadyForLove)
    {
      if (_mateSearchIndex >= _instances.Count)
        _mateSearchIndex = 0;

      // Find nearest available mate lazily over time
      if (_mateSearchIndex < _instances.Count)
      {
        CritterController potentialMate = _instances[_mateSearchIndex];
        if (potentialMate != this && potentialMate.IsReadyForLove)
        {
          float distToMate = Vector3.Distance(transform.position, potentialMate.transform.position);
          float distToNearestMate = _nearestMate != null ? Vector3.Distance(transform.position, _nearestMate.transform.position) : Mathf.Infinity;
          if (distToMate < distToNearestMate)
          {
            _nearestMate = potentialMate;
          }
        }

        ++_mateSearchIndex;
      }

      // Move towards current desired mate 
      if (_nearestMate != null)
      {
        Vector3 toMateVec = _nearestMate.transform.position - transform.position;
        _changeDirectionTimer = 1;
        _isMoving = true;
        _desiredDirection = toMateVec.normalized;

        // If someone gets to our potential mate first we have to try for someone else
        if (!_nearestMate.IsReadyForLove)
        {
          _nearestMate = null;
          return;
        }

        // If we get close enough, begin the process 
        float distToMate = toMateVec.magnitude;
        if (distToMate < kMinMateDistance)
        {
          MateWith(_nearestMate, isLeader: true);
        }
      }
    }

    Debug.DrawRay(transform.position, _desiredDirection, Color.blue);
  }

  private void MateWith(CritterController critter, bool isLeader)
  {
    _currentMate = critter;
    _isMoving = false;

    GameObject mateFx = Instantiate(_mateEffectPrefab, transform);
    mateFx.transform.localPosition = Vector3.zero;

    if (isLeader)
    {
      _nearestMate.MateWith(this, isLeader: false);
      StartCoroutine(MateAsync());
    }
  }

  private void StopMating()
  {
    _currentMate = null;
    _vigorLevel = 0;
    _changeDirectionTimer = 0;
  }

  private void SetSize(CritterConstants.CreatureSize newSize, bool animate)
  {
    _size = newSize;
    _vigorUI.gameObject.SetActive(((int)_size >= (int)kMinVigorSize));

    if (animate)
    {
      StartCoroutine(UpdateSizeAsync());
    }
    else
    {
      _visualRoot.localScale = Vector3.one * CritterConstants.GetCreatureSizeScale(_size);
    }
  }

  private IEnumerator MateAsync()
  {
    yield return new WaitForSeconds(3.0f);

    if (_currentMate != null)
    {
      EggController egg = Instantiate(_eggPrefab);
      egg.transform.position = (transform.position + _currentMate.transform.position) / 2;
      egg.transform.rotation = Random.rotationUniform;

      // Mix the DNA from the two parents and store in the child
      CreatureDescriptor childDNA = CreatureDescriptor.CreateCreatureDescriptorFromParents(this, _currentMate);
      egg.SetDNA(childDNA);

      _currentMate.StopMating();
    }

    StopMating();
  }

  private IEnumerator UpdateSizeAsync()
  {
    Vector3 startScale = _visualRoot.localScale;
    Vector3 endScale = Vector3.one * CritterConstants.GetCreatureSizeScale(_size);
    for (float time = 0; time < kGrowAnimationDuration; time += Time.deltaTime)
    {
      float t = time / kGrowAnimationDuration;
      float tCurve = _growUpCurve.Evaluate(t);
      _visualRoot.localScale = Vector3.LerpUnclamped(startScale, endScale, tCurve);
      yield return null;
    }
  }
}