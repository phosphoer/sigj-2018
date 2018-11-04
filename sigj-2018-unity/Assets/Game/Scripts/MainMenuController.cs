using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void OnNewGameClicked()
  {
    GameStateManager.Instance.SetGameStage(GameStateManager.GameStage.PreGame);
  }

  public void OnInstructionsClicked()
  {

  }

  public void OnSettingsClicked()
  {

  }

  public void OnQuitGameClicked()
  {
    //If we are running in a standalone build of the game
#if UNITY_STANDALONE
    //Quit the application
    Application.Quit();
#endif

    //If we are running in the editor
#if UNITY_EDITOR
    //Stop playing the scene
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }
}
