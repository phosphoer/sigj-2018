using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OnMainMenuClicked()
  {
    GameStateManager.Instance.SetGameStage(GameStateManager.GameStage.MainMenu);
  }
}
