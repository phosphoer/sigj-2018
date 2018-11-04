using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHatchController : MonoBehaviour
{
  public float HatchCheckDelay = 1.0f;
  public bool bRecentSpawn = false;
  public bool bCreatureBlocking = false;
  private Animator _animator;

  void Start()
  {
    CustomerOrderManager.Instance.RegisterInHatchController(this);
    _animator = GetComponent<Animator>();
    SetOpenState(false);
    StartCoroutine(CloseHatchAsync());
  }

  private void OnTriggerStay(Collider other)
  {
    bCreatureBlocking = other.GetComponentInParent<CritterController>() != null;
  }

  public void OnCreatureSpawned()
  {
    SetOpenState(true);
    bRecentSpawn = true;
  }

  public void SetOpenState(bool bIsOpen)
  {
    _animator.SetBool("IsOpen", bIsOpen);
  }

  private IEnumerator CloseHatchAsync()
  {
    while (true) {
      yield return new WaitForSeconds(HatchCheckDelay);

      SetOpenState(bRecentSpawn || bCreatureBlocking);
      bRecentSpawn = false;
      bCreatureBlocking = false;
    }
  }
}
