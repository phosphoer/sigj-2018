using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClockController : MonoBehaviour {

  public static string MORNING_START_TIME = "9AM";
  public static string LUNCH_START_TIME = "12PM";
  public static string AFTERNOON_START_TIME = "1PM";
  public static string[] TimeStrings = new string[] { "8AM", MORNING_START_TIME, "10AM", "11AM", LUNCH_START_TIME, AFTERNOON_START_TIME, "2PM", "3PM", "4PM", "5PM" };

  public float TotalRoundTimeSeconds= 30;

  private Material _gradientMaterial;
  private TMPro.TextMeshPro _textMesh;
  private float _startGameTime;
  private float _timeFraction;
  private IEnumerator _clockRefreshTimer;

  // Use this for initialization
  void Start()
  {
    _gradientMaterial= this.transform.Find("TimerGradient").GetComponent<MeshRenderer>().material;
    _textMesh = this.transform.Find("Textbox").GetComponent<TMPro.TextMeshPro>();

    _timeFraction = 0;
    RefreshTimeText();
    RefreshTimeGradient();

    GameStateManager.Instance.RegisterGameClockController(this);
  }

  public bool HasPregameFinished()
  {
    return _timeFraction >= GetFractionForTimeString(MORNING_START_TIME);
  }

  public bool HasMorningFinished()
  {
    return _timeFraction >= GetFractionForTimeString(LUNCH_START_TIME);
  }

  public bool HasLunchFinished()
  {
    return _timeFraction >= GetFractionForTimeString(AFTERNOON_START_TIME);
  }

  public bool HasWorkdayFinished()
  {
    return _timeFraction >= 1.0f;
  }

  public void StartClock()
  {
    StopClock();

    _startGameTime = Time.timeSinceLevelLoad;
    _timeFraction = 0;
    _clockRefreshTimer = ClockRefresh();

    StartCoroutine(_clockRefreshTimer);
  }

  public void StopClock()
  {
    if (_clockRefreshTimer != null) {
      StopCoroutine(_clockRefreshTimer);
      _clockRefreshTimer = null;
    }
  }

  IEnumerator ClockRefresh()
  {
    // StopClock() will kill this coroutine at the end of the round
    while (true) {
      yield return new WaitForSeconds(1.0f);

      RefreshTimeFraction();
      RefreshTimeText();
      RefreshTimeGradient();
    }
  }

  public void RefreshTimeFraction()
  {
    _timeFraction = Mathf.Clamp01( (Time.timeSinceLevelLoad - _startGameTime) / TotalRoundTimeSeconds);
  }

  private float GetFractionForTimeString(string timeString)
  {
    int timeStringIndex= System.Array.IndexOf(TimeStrings, timeString);
    return timeStringIndex != -1 ? (float)timeStringIndex / (float)TimeStrings.Length : 1.0f;
  }

  private void RefreshTimeText()
  {
    int timeStringIndex = System.Math.Min(System.Math.Max((int)(_timeFraction * (float)TimeStrings.Length), 0), TimeStrings.Length-1);
    string timeString = TimeStrings[timeStringIndex];

    _textMesh.SetText(timeString);
  }

  private void RefreshTimeGradient()
  {
    _gradientMaterial.SetFloat("_Cutoff", Mathf.Max(1.0f - _timeFraction, 0.01f));
  }
}
