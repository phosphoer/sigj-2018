using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutHatchController : MonoBehaviour
{

  private Animator _animator;

  void Start()
  {
    CustomerOrderManager.Instance.RegisterOutHatchController(this);
    _animator = GetComponent<Animator>();
  }

  private void OnTriggerEnter(Collider other)
  {
    CritterController Critter = other.GetComponentInParent<CritterController>();
    if (Critter != null)
    {
      CustomerOrderManager.Instance.OnCreatureDeposited(Critter);
    }
  }

  public void SetOpenState(bool bIsOpen)
  {
    _animator.SetBool("IsOpen", bIsOpen);
  }
}
