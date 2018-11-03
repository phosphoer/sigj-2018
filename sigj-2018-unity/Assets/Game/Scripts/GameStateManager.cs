using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager> {
  public enum GameStage
  {
    Invalid,
    MainMenu,
    PreGame,
    Morning,
    Lunchtime,
    Afternoon,
    PostGame
  }

  public GameObject PlayerControllerPrefab;
  public GameObject MainMenuPrefab;

  private GameStage _gameStage = GameStage.Invalid;
  private GameObject _menuMenu = null;
  private GameObject _playerController = null;

  private void Awake()
  {
    GameStateManager.Instance = this;
  }

  // Use this for initialization
  void Start () {
    SetGameStage(GameStage.PreGame);
  }
	
	// Update is called once per frame
	void Update () {
    GameStage nextGameStage = _gameStage;

    switch (_gameStage) {
      case GameStage.MainMenu:
        break;
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
      case GameStage.MainMenu: 
        {
          Destroy(_menuMenu);
          _menuMenu = null;
        }
        break;
      case GameStage.PreGame: 
        {
        }
        break;
      case GameStage.Morning: 
        {
          Destroy(_playerController);
          _playerController = null;
        }
        break;
      case GameStage.Lunchtime: {
          // TODO: Destroy lunchtime UI 
        }
        break;
      case GameStage.Afternoon: {
          Destroy(_playerController);
          _playerController = null;
        }
        break;
      case GameStage.PostGame:
        break;
    }
  }

  public void OnEnterStage(GameStage newGameStage)
  {
    switch (newGameStage) {
      case GameStage.MainMenu: {
          _menuMenu = (GameObject)Instantiate(MainMenuPrefab, Vector3.zero, Quaternion.identity);
        }
        break;
      case GameStage.PreGame: 
        {
          _playerController = (GameObject)Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
          CustomerOrderManager.Instance.OnRoundStarted();
        }
        break;
      case GameStage.Morning: 
        {

        }
        break;
      case GameStage.Lunchtime: 
        {
          //TODO: Spawn lunch time UI
        }
        break;
      case GameStage.Afternoon: 
        {
          _playerController = (GameObject)Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
        }
        break;
      case GameStage.PostGame: 
        {
          CustomerOrderManager.Instance.OnRoundCompleted();
          //TODO: Spawn post game UI
        }
        break;
    }
  }
}
