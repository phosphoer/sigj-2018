using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager> {
  public enum GameStage
  {
    Invalid,
    PreGame,
    Morning,
    Lunchtime,
    Afternoon,
    PostGame
  }

  public GameObject MainMenuPrefab;

  private GameStage _gameStage = GameStage.Invalid;
  private GameObject _menuMenu = null;

  private void Awake()
  {
    GameStateManager.Instance = this;
  }

  // Use this for initialization
  void Start () {
    SetGameStage(GameStage.Morning);
  }
	
	// Update is called once per frame
	void Update () {
    GameStage nextGameStage = _gameStage;

    switch (_gameStage) {
      case GameStage.PreGame:
        break;
      case GameStage.Morning:
        break;
      case GameStage.Lunchtime:
        break;
      case GameStage.Afternoon:
        break;
      case GameStage.PostGame:
        break;
    }

    SetGameStage(nextGameStage);
  }

  public void SetGameStage(GameStage newGameStage)
  {
    if (newGameStage != _gameStage) {
      OnExitStage(_gameStage);
      OnEnterStage(newGameStage);
      _gameStage = newGameStage;
    }
  }

  public void OnExitStage(GameStage oldGameStage)
  {
    switch(oldGameStage) {
      case GameStage.PreGame: 
        {
          Destroy(_menuMenu);
          _menuMenu = null;
        }
        break;
      case GameStage.Morning:
        break;
      case GameStage.Lunchtime:
        break;
      case GameStage.Afternoon:
        break;
      case GameStage.PostGame:
        break;
    }
  }

  public void OnEnterStage(GameStage newGameStage)
  {
    switch (newGameStage) {
      case GameStage.PreGame: 
        {
          _menuMenu = (GameObject)Instantiate(MainMenuPrefab, Vector3.zero, Quaternion.identity);
        }
        break;
      case GameStage.Morning:
        CustomerOrderManager.Instance.OnRoundStarted();
        break;
      case GameStage.Lunchtime:
        break;
      case GameStage.Afternoon:
        break;
      case GameStage.PostGame:
        CustomerOrderManager.Instance.OnRoundCompleted();
        break;
    }
  }
}
