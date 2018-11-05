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
  public GameObject EndMenuPrefab;
  public GameObject SandwichPrefab;
  public SoundBank MusicMenuLoop;
  public SoundBank MusicGameLoop;

  private GameStage _gameStage = GameStage.Invalid;
  private GameObject _mainMenu = null;
  private GameObject _endMenu = null;
  private GameObject _playerController = null;
  private GameClockController _gameClockController = null;
  private GameObject _sandwich = null;

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
          if (_gameClockController.HasLunchFinished())
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
          AudioManager.Instance.FadeOutSound(gameObject, MusicMenuLoop, 3.0f);
          Destroy(_mainMenu);
          _mainMenu = null;
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
          if (_sandwich != null) {
            Destroy(_sandwich);
            _sandwich = null;
          }
        }
        break;
      case GameStage.Afternoon:
        {
          Destroy(_playerController);
          _playerController = null;
        }
        break;
      case GameStage.PostGame:
        {
          Destroy(_endMenu);
          _endMenu = null;
        }
        break;
    }
  }

  public void OnEnterStage(GameStage newGameStage)
  {
    switch (newGameStage)
    {
      case GameStage.MainMenu:
        {
          _mainMenu = (GameObject)Instantiate(MainMenuPrefab, Vector3.zero, Quaternion.identity);
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
          if (SandwichPrefab != null) {
            _sandwich= (GameObject)Instantiate(SandwichPrefab, Camera.main.gameObject.transform.position + Vector3.forward * 0.75f, Quaternion.Euler(90.0f, 0.0f, 0.0f));
            _sandwich.GetComponent<Rigidbody>().useGravity = false;
            _sandwich.GetComponent<Rigidbody>().isKinematic = true;
            _sandwich.GetComponent<Sandwich>().SetIsSandwichClickable();
          }
        }
        break;
      case GameStage.Afternoon:
        {
          _playerController = (GameObject)Instantiate(PlayerControllerPrefab, Vector3.zero, Quaternion.identity);
        }
        break;
      case GameStage.PostGame:
        {
          CritterController.DestroyAllCreatures();

          if (_gameClockController != null)
          {
            _gameClockController.StopClock();
          }

          CustomerOrderManager.Instance.OnRoundCompleted();
          AudioManager.Instance.FadeOutSound(gameObject, MusicGameLoop, 3.0f);
          //TODO: Spawn post game UI

          _endMenu = (GameObject)Instantiate(EndMenuPrefab, new Vector3(Camera.main.gameObject.transform.position.x, 2.65f, -3.24f), Quaternion.identity);
        }
        break;
    }
  }
}
