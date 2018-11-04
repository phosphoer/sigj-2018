using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHatchController : MonoBehaviour
{
  public GameObject GoodOrderLight;
  public GameObject BadOrderLight;
  public GameObject GoodOrderEffectPrefab;
  public GameObject BadOrderEffectPrefab;
  public Transform OrderEffectTransform;

  private Animator _animator;

  void Start()
  {
    CustomerOrderManager.Instance.RegisterInHatchController(this);
    _animator = GetComponent<Animator>();
    TurnOffLights();
  }

  private void OnTriggerEnter(Collider other)
  {
    CritterController Critter = other.GetComponentInParent<CritterController>();
    if (Critter != null)
    {
      CustomerOrderManager.Instance.OnCreatureDeposited(Critter);
    }
  }

  public void OnCrittedScored(bool bOrderSatisfied)
  {
    GameObject EffectPrefab = bOrderSatisfied ? GoodOrderEffectPrefab : BadOrderEffectPrefab;
    GameObject OrderLight = bOrderSatisfied ? GoodOrderLight : BadOrderLight;

    if (EffectPrefab != null) {
      Instantiate(EffectPrefab, OrderEffectTransform.position, OrderEffectTransform.rotation);
    }

    if (OrderLight != null) {
      OrderLight.SetActive(true);
      StartCoroutine(TurnOffLightsAsync());
    }
  }

  public void SetOpenState(bool bIsOpen)
  {
    _animator.SetBool("IsOpen", bIsOpen);
  }

  private IEnumerator TurnOffLightsAsync()
  {
    yield return new WaitForSeconds(1.0f);

    TurnOffLights();
  }

  private void TurnOffLights()
  {
    if (GoodOrderLight != null)
      GoodOrderLight.SetActive(false);

    if (BadOrderLight != null)
      BadOrderLight.SetActive(false);
  }
}
