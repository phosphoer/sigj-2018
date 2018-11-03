using UnityEngine;
using System.Collections;

public class EggController : MonoBehaviour
{
  [SerializeField]
  private GameObject _spawnEffectPrefab = null;

  [SerializeField]
  private GameObject _visualRoot = null;

  [SerializeField]
  private Rigidbody _rigidBody = null;

  private void Start()
  {
    StartCoroutine(SpawnAsync());
  }

  private IEnumerator SpawnAsync()
  {
    _visualRoot.SetActive(false);
    _rigidBody.isKinematic = true;
    GameObject spawnFx = Instantiate(_spawnEffectPrefab, transform.position, Quaternion.identity);

    yield return new WaitForSeconds(1.0f);

    _rigidBody.isKinematic = false;
    _rigidBody.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
    _visualRoot.SetActive(true);
  }
}