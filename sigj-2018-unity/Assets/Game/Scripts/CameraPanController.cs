using UnityEngine;

public class CameraPanController : MonoBehaviour
{
  [SerializeField]
  private RangedFloat _panningViewThresholds = new RangedFloat(0.2f, 0.8f);

  [SerializeField]
  private RangedFloat _panningWorldBounds = new RangedFloat(-5, 5);

  [SerializeField]
  private float _panningSpeed = 1.0f;

  private void Update()
  {
    Vector3 targetCameraPos = Camera.main.transform.position;
    Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    float distFromViewportCenter = _panningViewThresholds.DistanceFromRange(viewportPos.x);

    targetCameraPos.x += distFromViewportCenter * _panningSpeed;
    targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, _panningWorldBounds.MinValue, _panningWorldBounds.MaxValue);

    float panSpeed = Time.deltaTime * _panningSpeed;
    Camera.main.transform.position = Mathfx.Damp(Camera.main.transform.position, targetCameraPos, 0.5f, panSpeed);
  }
}