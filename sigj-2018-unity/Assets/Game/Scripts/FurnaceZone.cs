using UnityEngine;
using System.Collections;
using System;

public class FurnaceZone : MonoBehaviour
{
  [SerializeField]
  private GameObject _burninateEffectPrefab = null;

  [SerializeField]
  private Transform _burnEffectSpawnAnchor = null;

  [SerializeField]
  private ImpactReaction _furnaceTrigger = null;

  private void Start()
  {
    _furnaceTrigger.ImpactBegin += OnFurnaceTrigger;
  }

  private void OnDestroy()
  {
    _furnaceTrigger.ImpactBegin -= OnFurnaceTrigger;
  }

  private void OnFurnaceTrigger(GameObject other, bool isTrigger)
  {
    var destroyable = other.GetComponentInParent<DestroyableByFurnace>();
    if (destroyable != null)
    {
      Destroy(destroyable.gameObject);

      GameObject burnFx = Instantiate(_burninateEffectPrefab, _burnEffectSpawnAnchor.position, _burnEffectSpawnAnchor.rotation);
    }
  }
}