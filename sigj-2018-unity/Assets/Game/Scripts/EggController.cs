using UnityEngine;
using System.Collections;

public class EggController : MonoBehaviour
{
  [SerializeField]
  private GameObject _spawnEffectPrefab = null;

  [SerializeField]
  private GameObject _hatchEffectPrefab = null;

  [SerializeField]
  private ProgressBarUI _hatchProgressUIPrefab = null;

  [SerializeField]
  private AnimationCurve _hatchScaleCurve = null;

  [SerializeField]
  private GameObject _visualRoot = null;

  [SerializeField]
  private Rigidbody _rigidBody = null;

  [SerializeField]
  private RangedFloat _rockTimeRange = new RangedFloat(2.0f, 5.0f);

  [SerializeField]
  private RangedFloat _rockStrengthRange = new RangedFloat(0.25f, 0.5f);

  [SerializeField]
  private RangedFloat _hatchTimeRange = new RangedFloat(15.0f, 20.0f);

  private float _rockTimer;
  private float _hatchTimer;
  private bool _isHatching;
  private bool _isSpawned;
  private float _hatchTimeTotal;
  private ProgressBarUI _hatchProgressUI;

  private void Start()
  {
    StartCoroutine(SpawnAsync());
  }

  private void Update()
  {
    // Occaisonally rock the egg
    _rockTimer -= Time.deltaTime;
    if (_rockTimer < 0)
    {
      _rockTimer = _rockTimeRange.RandomValue;
      _rigidBody.AddTorque(Random.onUnitSphere * _rockStrengthRange.RandomValue, ForceMode.Impulse);
    }

    // Progress towards hatching
    if (_isSpawned && !_isHatching)
    {
      _hatchTimer = Mathf.Max(_hatchTimer - Time.deltaTime, 0);
      _hatchProgressUI.transform.position = transform.position + Vector3.up;
      _hatchProgressUI.FillPercent = 1.0f - (_hatchTimer / _hatchTimeTotal);

      if (_hatchTimer <= 0)
      {
        StartCoroutine(HatchAsync());
      }
    }
  }

  private IEnumerator SpawnAsync()
  {
    _visualRoot.SetActive(false);
    _rigidBody.isKinematic = true;
    Instantiate(_spawnEffectPrefab, transform.position, Quaternion.identity);

    yield return new WaitForSeconds(1.0f);

    _hatchTimeTotal = _hatchTimeRange.RandomValue;
    _hatchTimer = _hatchTimeTotal;
    _rigidBody.isKinematic = false;
    _visualRoot.SetActive(true);

    _hatchProgressUI = Instantiate(_hatchProgressUIPrefab, transform.position, Quaternion.identity);
    _isSpawned = true;
  }

  private IEnumerator HatchAsync()
  {
    _isHatching = true;
    Destroy(_hatchProgressUI.gameObject);

    Vector3 startScale = _visualRoot.transform.localScale;
    Vector3 endScale = Vector3.one * 1.5f;
    float hatchDuration = 1.0f;
    for (float time = 0; time < hatchDuration; time += Time.deltaTime)
    {
      float t = time / hatchDuration;
      float tCurve = _hatchScaleCurve.Evaluate(t);
      _visualRoot.transform.localScale = Vector3.LerpUnclamped(startScale, endScale, tCurve);
      yield return null;
    }

    yield return new WaitForSeconds(1.0f);
    Instantiate(_hatchEffectPrefab, transform.position, Quaternion.identity);

    Destroy(gameObject);
  }
}