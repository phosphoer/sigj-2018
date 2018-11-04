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
  private GameObject _duplicateEffectToggle = null;

  [SerializeField]
  private Transform _duplicateSpawnAnchor = null;

  [SerializeField]
  private ImpactReaction _duplicateTrigger = null;

  [SerializeField]
  private float _coolDownTime = 10.0f;

  [SerializeField]
  private TMPro.TMP_Text _coolDownText = null;

  private float _coolDown;
  private int _roundedCoolDown;
  private bool _isDuplicating;

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
    if (!_isDuplicating)
    {
      _coolDown = Mathf.Max(_coolDown - Time.deltaTime, 0);
      if (_coolDownText != null)
      {
        int roundedTime = Mathf.CeilToInt(_coolDown);
        if (roundedTime != _roundedCoolDown)
        {
          _roundedCoolDown = roundedTime;
          _coolDownText.text = roundedTime > 0 ? roundedTime.ToString() : "Ready";
        }
      }
    }
  }

  private void OnDuplicateTrigger(GameObject other, bool isTrigger)
  {
    if (_coolDown <= 0)
    {
      var duplicatable = other.GetComponentInParent<Duplicatable>();
      if (duplicatable != null)
      {
        _coolDown = _coolDownTime;
        StartCoroutine(DuplicateAsync(duplicatable));
      }
    }
  }

  private IEnumerator DuplicateAsync(Duplicatable duplicatable)
  {
    _isDuplicating = true;

    if (_coolDownText != null)
      _coolDownText.text = "Clone";

    if (_duplicateEffectToggle != null)
      _duplicateEffectToggle.SetActive(true);

    if (_duplicateEffectPrefab != null)
    {
      Instantiate(_duplicateEffectPrefab, _duplicateEffectSpawnAnchor.position, _duplicateEffectSpawnAnchor.rotation);
    }

    yield return new WaitForSeconds(3.0f);

    GameObject dupe = duplicatable.CreateDuplicate();
    dupe.transform.SetPositionAndRotation(_duplicateSpawnAnchor.position, _duplicateSpawnAnchor.rotation);
    dupe.transform.SetParent(duplicatable.transform.parent);

    yield return new WaitForSeconds(1.0f);

    if (_duplicateEffectToggle != null)
      _duplicateEffectToggle.SetActive(false);

    _isDuplicating = false;
  }
}