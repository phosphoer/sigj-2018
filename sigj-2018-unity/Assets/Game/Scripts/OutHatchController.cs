using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutHatchController : MonoBehaviour
{
  public GameObject GoodOrderLight;
  public GameObject BadOrderLight;
  public GameObject GoodOrderEffectPrefab;
  public GameObject BadOrderEffectPrefab;
  public Transform OrderEffectTransform;
  public SoundBank OpenSound;
  public SoundBank CloseSound;
  public SoundBank CorrectSound;
  public SoundBank IncorrectSound;

  private Animator _animator;
  private bool _wasOpen;

  void Start()
  {
    CustomerOrderManager.Instance.RegisterOutHatchController(this);
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
    SoundBank ScoreSound = bOrderSatisfied ? CorrectSound : IncorrectSound;

    if (EffectPrefab != null)
    {
      Instantiate(EffectPrefab, OrderEffectTransform.position, OrderEffectTransform.rotation);
    }

    AudioManager.Instance.PlaySound(ScoreSound);

    if (OrderLight != null)
    {
      OrderLight.SetActive(true);
      StartCoroutine(TurnOffLightsAsync());
    }
  }

  public void SetOpenState(bool bIsOpen)
  {
    _animator.SetBool("IsOpen", bIsOpen);

    if (bIsOpen != _wasOpen)
    {
      if (bIsOpen) AudioManager.Instance.PlaySound(OpenSound);
      else AudioManager.Instance.PlaySound(CloseSound);

      _wasOpen = bIsOpen;
    }
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
