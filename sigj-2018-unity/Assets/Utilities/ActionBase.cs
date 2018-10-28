using UnityEngine;
using System.Collections;

public abstract class ActionBase : MonoBehaviour
{
  public Coroutine StartAction()
  {
    Debug.LogFormat("Starting action {0} in {1}", name, transform.parent.name);

    return StartCoroutine(DoActionAsync());
  }

  public virtual void ResetActionState()
  {

  }

  protected abstract IEnumerator DoActionAsync();
}