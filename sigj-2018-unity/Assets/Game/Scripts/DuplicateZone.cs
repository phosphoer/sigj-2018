using UnityEngine;
using System.Collections;
using System;

public class DuplicateZone : MonoBehaviour
{
  [SerializeField]
  private GameObject _duplicateEffectPrefab = null;

  [SerializeField]
  private Transform _duplicateEffectSpawnAnchor = null;

  [SerializeField]
  private Transform _duplicateSpawnAnchor = null;

  [SerializeField]
  private ImpactReaction _duplicateTrigger = null;

  [SerializeField]
  private float _coolDownTime = 3.0f;

  private float _coolDown;

  private void Start()
  {
    _duplicateTrigger.ImpactBegin += OnDuplicateTrigger;
  }

  private void OnDestroy()
  {
    _duplicateTrigger.ImpactBegin -= OnDuplicateTrigger;
  }

  private void Update()
  {
    _coolDown -= Time.deltaTime;
  }

  private void OnDuplicateTrigger(GameObject other, bool isTrigger)
  {
    if (_coolDown <= 0)
    {
      var duplicatable = other.GetComponentInParent<Duplicatable>();
      if (duplicatable != null)
      {
        _coolDown = _coolDownTime;
        Instantiate(_duplicateEffectPrefab, _duplicateEffectSpawnAnchor.position, _duplicateEffectSpawnAnchor.rotation);

        GameObject dupe = duplicatable.CreateDuplicate();
        dupe.transform.SetPositionAndRotation(_duplicateSpawnAnchor.position, _duplicateSpawnAnchor.rotation);
        dupe.transform.SetParent(duplicatable.transform.parent);
      }
    }
  }
}