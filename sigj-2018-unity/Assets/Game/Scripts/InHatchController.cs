using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHatchController : MonoBehaviour
{
  public float SpawnCheckDelay = 1.0f;
  public float HatchCheckDelay = 1.0f;
  public bool bRecentSpawn = false;
  public bool bCreatureBlocking = false;
  public SoundBank OpenSound;
  public SoundBank CloseSound;
  private Animator _animator;
  private bool _wasOpen;
  private Queue<CreatureDescriptor> _pendingCreatureSpawns = new Queue<CreatureDescriptor>();

  void Start()
  {
    CustomerOrderManager.Instance.RegisterInHatchController(this);
    _animator = GetComponent<Animator>();
    SetOpenState(false);
    StartCoroutine(CloseHatchAsync());
    StartCoroutine(SpawnCreatureAsync());
  }

  private void OnTriggerStay(Collider other)
  {
    bCreatureBlocking = other.GetComponentInParent<CritterController>() != null;
  }

  public bool IsHatchOpened()
  {
    return _animator.GetCurrentAnimatorStateInfo(0).IsName("OpenIdle");
  }

  public void SpawnCreature(CreatureDescriptor creatureDNA)
  {
    SetOpenState(true);
    bRecentSpawn = true;

    if (IsHatchOpened())
    {
      // Spawn a creature that corresponds to that order
      CritterSpawner.Instance?.SpawnCritter(creatureDNA, null);
    }
    else
    {
      _pendingCreatureSpawns.Enqueue(creatureDNA);
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

  private IEnumerator CloseHatchAsync()
  {
    while (true)
    {
      yield return new WaitForSeconds(HatchCheckDelay);

      SetOpenState(bRecentSpawn || bCreatureBlocking);
      bRecentSpawn = false;
      bCreatureBlocking = false;
    }
  }

  private IEnumerator SpawnCreatureAsync()
  {
    while (true)
    {
      yield return new WaitForSeconds(SpawnCheckDelay);

      if (_pendingCreatureSpawns.Count > 0 && IsHatchOpened())
      {

        CreatureDescriptor creatureDNA = _pendingCreatureSpawns.Dequeue();

        SetOpenState(true);
        bRecentSpawn = true;

        // Spawn a creature that corresponds to that order
        CritterSpawner.Instance?.SpawnCritter(creatureDNA, null);
      }
    }
  }
}
