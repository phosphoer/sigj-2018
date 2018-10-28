using UnityEngine;

public class IgnoreChildCollisions : MonoBehaviour
{
  private void Start()
  {
    Collider[] colliders = GetComponentsInChildren<Collider>();
    for (int i = 0; i < colliders.Length; ++i)
    {
      for (int j = i + 1; j < colliders.Length; ++j)
      {
        Physics.IgnoreCollision(colliders[i], colliders[j], true);
      }
    }
  }
}