using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour
{
  [SerializeField]
  private Text _fpsTextUI;

  private float _fps;

  private void OnEnable()
  {
    StartCoroutine(UpdateFPS());
  }

  private void Update()
  {
    _fps = Mathfx.Damp(_fps, 1.0f / Time.smoothDeltaTime, 0.25f, Time.deltaTime * 5.0f);

    if (Input.GetKeyDown(KeyCode.F1))
    {
      _fpsTextUI.enabled = !_fpsTextUI.enabled;
    }
  }

  private IEnumerator UpdateFPS()
  {
    while (gameObject.activeInHierarchy)
    {
      _fpsTextUI.text = ((int)_fps).ToString();
      yield return new WaitForSeconds(1.0f);
    }
  }
}