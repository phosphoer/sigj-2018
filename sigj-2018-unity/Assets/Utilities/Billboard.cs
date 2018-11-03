using UnityEngine;

public class Billboard : MonoBehaviour
{
  public bool FlipZ;
  public bool IgnoreY;

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
    Vector3 pos = cam.transform.position;
    Vector3 toCamera = pos - transform.position;
    if (IgnoreY)
    {
      toCamera.y = 0;
    }

    transform.rotation = Quaternion.LookRotation(toCamera.normalized * (FlipZ ? 1.0f : -1.0f), Vector3.up);
  }
}