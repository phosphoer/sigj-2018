using UnityEngine;

public class ConsistentScreenSize : MonoBehaviour
{
  public float Size = 1;

  private void OnEnable()
  {
    CameraPreRender.PreRender += OnCameraPreRender;
  }

  private void OnDisable()
  {
    CameraPreRender.PreRender -= OnCameraPreRender;
  }

  private void OnCameraPreRender(Camera cam)
  {
    Vector3 cameraRightLocal = transform.InverseTransformDirection(cam.transform.right);
    Vector3 cameraUpLocal = transform.InverseTransformDirection(cam.transform.up);

    transform.localScale = Vector3.one;
    Vector3 screenCorner1 = cam.WorldToViewportPoint(transform.TransformPoint(-cameraRightLocal + cameraUpLocal));
    Vector3 screenCorner2 = cam.WorldToViewportPoint(transform.TransformPoint(cameraRightLocal - cameraUpLocal));

    float viewSize = Vector3.Distance(screenCorner1, screenCorner2);
    float scaleFactor = Size / viewSize;
    transform.localScale = Vector3.one * scaleFactor;
  }
}