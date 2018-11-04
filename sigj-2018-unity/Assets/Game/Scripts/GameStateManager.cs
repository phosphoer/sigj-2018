using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
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
  public SoundBank MusicMenuLoop;
  public SoundBank MusicGameLoop;

  private GameStage _gameStage = GameStage.Invalid;
  private GameObject _menuMenu = null;
  private GameObject _playerController = null;
  private GameClockController _gameClockController = null;

  private void Awake()
  {
    GameStateManager.Instance = this;
  }

  // Use this for initialization
  void Start()
  {
    SetGameStage(GameStage.MainMenu);
  }

  // Update is called once per frame
  void Update()
  {
    GameStage nextGameStage = _gameStage;

    switch (_gameStage)
    {
      case GameStage.MainMenu:
        break;
      case GameStage.PreGame:
        if (_gameClockController != null)
        {
          if (_gameClockController.HasPregameFinished())
          {
            nextGameStage = GameStage.Morning;
          }
        }
        break;
      case GameStage.Morning:
        if (_gameClockController != null)
        {
          if (_gameClockController.HasMorningFinished())
          {
            nextGameStage = GameStage.Lunchtime;
          }
        }
        break;
      case GameStage.Lunchtime:
        if (_gameClockController != null)
        {
          if (_gameClockController.HasMorningFinished())
          {
            nextGameStage = GameStage.Afternoon;
          }
        }
        break;
      case GameStage.Afternoon:
        if (_gameClockController != null)
        {
          if (_gameClockController.HasWorkdayFinished())
          {
            nextGameStage = GameStage.PostGame;
          }
        }
        break;
      case GameStage.PostGame:
        break;
    }

    SetGameStage(nextGameStage);
  }

  public void SetGameStage(GameStage newGameStage)
  {
    if (newGameStage != _gameStage)
    {
      OnExitStage(_gameStage);
      OnEnterStage(newGameStage);
      _gameStage = newGameStage;
    }
  }

  public void RegisterGameClockController(GameClockController clockController)
  {
    _gameClockController = clockController;
  }

  public void OnExitStage(GameStage oldGameStage)
  {
    switch (oldGameStage)
    {
      case GameStage.MainMenu:
        {
          Destroy(_menuMenu);
          _menuMenu = null;
          AudioManager.Instance.FadeOutSound(gameObject, MusicMenuLoop, 3.0f);
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
      case GameStage.Lunchtime:
        {
          // TODO: Destroy lunchtime UI 
        }
        break;
      case GameStage.Afternoon:
        {
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
    switch (newGameStage)
    {
      case GameStage.MainMenu:
        {
          _menuMenu = (GameObject)Instantiate(MainMenuPrefab, Vector3.zero, Quaternion.identity);
          AudioManager.Instance.FadeInSound(gameObject, MusicMenuLoop, 3.0f);
        }
        break;
      case GameStage.PreGame:
        {
          _playerController = (GameObject)Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
          AudioManager.Instance.FadeInSound(gameObject, MusicGameLoop, 3.0f);
          CustomerOrderManager.Instance.OnRoundStarted();

          if (_gameClockController != null)
          {
            _gameClockController.StartClock();
          }
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
          if (_gameClockController != null)
          {
            _gameClockController.StopClock();
          }

          CustomerOrderManager.Instance.OnRoundCompleted();
          AudioManager.Instance.FadeOutSound(gameObject, MusicGameLoop, 3.0f);
          //TODO: Spawn post game UI
        }
        break;
    }
  }
}
