using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{
  public float FillPercent
  {
    get { return _fillRoot.localScale.x; }
    set { _fillRoot.localScale = _fillRoot.localScale.WithX(value); }
  }

  [SerializeField]
  private Transform _fillRoot = null;
}