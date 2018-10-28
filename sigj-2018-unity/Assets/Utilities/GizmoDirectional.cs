using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GizmoDirectional : MonoBehaviour
{
  public float Scale = 1.0f;
  public Color Color = Color.white;

#if UNITY_EDITOR

  private void OnDrawGizmos()
  {
    Handles.color = Color;
    Handles.DrawLine(transform.position + transform.right * Scale * 0.5f, transform.position + transform.forward * Scale * 2.0f);
    Handles.DrawLine(transform.position - transform.right * Scale * 0.5f, transform.position + transform.forward * Scale * 2.0f);
    Handles.DrawLine(transform.position - transform.right * Scale * 0.5f, transform.position + transform.right * Scale * 0.5f);

    Handles.DrawLine(transform.position + transform.right * Scale * 0.51f, transform.position + transform.forward * Scale * 2.1f);
    Handles.DrawLine(transform.position - transform.right * Scale * 0.51f, transform.position + transform.forward * Scale * 2.1f);
    Handles.DrawLine(transform.position - transform.right * Scale * 0.51f, transform.position + transform.right * Scale * 0.51f);
  }
#endif
}