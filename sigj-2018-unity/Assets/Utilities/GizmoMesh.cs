using UnityEngine;

public class GizmoMesh : MonoBehaviour
{
  public float Scale = 1.0f;
  public Mesh Mesh;

#if UNITY_EDITOR

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireMesh(Mesh, transform.position, transform.rotation, transform.localScale * Scale);
  }
#endif
}