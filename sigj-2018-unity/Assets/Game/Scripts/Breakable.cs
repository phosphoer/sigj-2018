using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

  public GameObject DisableOnBreak;
  public GameObject EnableOnBreak;
  public Rigidbody ToDisable;
  public float BreakingPoint = 2f;
  public SoundBank BreakSound;

  bool BROKEN = false;


  public void OnCollisionEnter(Collision col)
  {
    if (col.impulse.magnitude >= BreakingPoint)
    {
      BROKEN = true;
      if (DisableOnBreak) DisableOnBreak.SetActive(false);
      if (EnableOnBreak) EnableOnBreak.SetActive(true);
      if (ToDisable) ToDisable.isKinematic = true;

      if (BreakSound != null)
        AudioManager.Instance.PlaySound(BreakSound);
    }
  }
}
