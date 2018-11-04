using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutHatchController : MonoBehaviour {

  private Animator _animator;

  // Use this for initialization
  void Start () {
    CustomerOrderManager.Instance.RegisterOutHatchController(this);
    _animator = GetComponent<Animator>();
  }
	
	// Update is called once per frame
	void Update () {
		
	}

  private void OnTriggerEnter(Collider other)
  {
    CritterController Critter= other.gameObject.GetComponent<CritterController>();
    if (Critter != null) {
      CustomerOrderManager.Instance.OnCreatureDeposited(Critter);
    }
  }

  public void SetOpenState(bool bIsOpen)
  {
    _animator.SetBool("IsOpen", bIsOpen);
  }
}
