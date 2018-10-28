using UnityEngine;

public class ImpactReaction : MonoBehaviour
{
  public enum CollisionType
  {
    Physical,
    Trigger,
    Both
  }

  public CollisionType Type;
  public LayerMask CollideWith;
  public string ComponentFilter;
  public bool DestroyComponentOnImpact;
  public bool DestroyGameObjectOnImpact;

  public event System.Action<GameObject, bool> ImpactBegin;
  public event System.Action<GameObject, bool> ImpactEnd;

  private void OnTriggerEnter(Collider c)
  {
    if (ValidateCollision(c.gameObject))
    {
      if (ImpactBegin != null)
        ImpactBegin(c.gameObject, true);

      BaseImpact();
    }
  }

  private void OnCollisionEnter(Collision c)
  {
    if (ValidateCollision(c.gameObject))
    {
      if (ImpactBegin != null)
        ImpactBegin(c.gameObject, false);

      BaseImpact();
    }
  }

  private void OnTriggerExit(Collider c)
  {
    if (ValidateCollision(c.gameObject))
    {
      if (ImpactEnd != null)
        ImpactEnd(c.gameObject, true);
    }
  }

  private void OnCollisionExit(Collision c)
  {
    if (ValidateCollision(c.gameObject))
    {
      if (ImpactEnd != null)
        ImpactEnd(c.gameObject, false);
    }
  }

  private void BaseImpact()
  {
    if (DestroyComponentOnImpact)
    {
      Destroy(this);
    }
    else if (DestroyGameObjectOnImpact)
    {
      Destroy(gameObject);
    }
  }

  protected virtual bool ValidateCollision(GameObject obj)
  {
    if ((CollideWith.value & (1 << obj.layer)) != 0)
    {
      if ((Type == CollisionType.Trigger || Type == CollisionType.Both))
      {
        if (!string.IsNullOrEmpty(ComponentFilter))
          return obj.GetComponent(ComponentFilter) != null;
        else
          return true;
      }
    }

    return false;
  }

}