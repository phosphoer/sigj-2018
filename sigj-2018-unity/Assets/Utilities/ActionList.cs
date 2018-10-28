using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionList : MonoBehaviour
{
  public enum LifetimeModeType
  {
    GameObject,
    Behaviour,
  }

  public bool BeginActionsOnStart;
  public bool LoopActions;
  public LifetimeModeType ActionLifetimeMode;

  private bool _cancelled;
  private List<ActionBase> _actions = new List<ActionBase>();

  public Coroutine StartActions()
  {
    _cancelled = false;
    return StartCoroutine(ExecuteActionsAsync());
  }

  public void StopActions()
  {
    _cancelled = true;
  }

  private void Awake()
  {
    foreach (Transform child in transform)
    {
      ActionBase action = child.GetComponent<ActionBase>();
      if (action != null)
      {
        _actions.Add(action);
        SetActionEnabled(action, false);
      }
    }

    _actions.Sort((a, b) =>
    {
      return a.transform.GetSiblingIndex() - b.transform.GetSiblingIndex();
    });
  }

  private void OnEnable()
  {
    if (BeginActionsOnStart)
    {
      StartActions();
    }
  }

  private void SetActionEnabled(ActionBase action, bool isEnabled)
  {
    if (ActionLifetimeMode == LifetimeModeType.Behaviour)
    {
      action.enabled = isEnabled;
    }
    else if (ActionLifetimeMode == LifetimeModeType.GameObject)
    {
      action.gameObject.SetActive(isEnabled);
    }
  }

  private IEnumerator ExecuteActionsAsync()
  {
    do
    {
      foreach (ActionBase action in _actions)
      {
        yield return null;

        if (_cancelled)
        {
          yield break;
        }

        if (action != null)
        {
          SetActionEnabled(action, true);
          action.ResetActionState();

          yield return action.StartAction();

          SetActionEnabled(action, false);
        }
      }

      yield return null;
    } while (LoopActions && !_cancelled);
  }
}