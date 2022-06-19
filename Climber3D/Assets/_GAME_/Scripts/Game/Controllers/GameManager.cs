using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] GameSettings gameSettings;

    #region Locals
    GameStates _gameState = GameStates.Loading;
    PlayerController _player;
    #endregion

    #region Properties
    public GameStates GameState => _gameState;
    public PlayerController Player { get { return _player; } set { _player = value; } }
    public GameSettings GameSettings => gameSettings;
    #endregion

    //TODO TUTORIAL public static bool IsTutorial = false;

    public enum GameStates
    {
        Loading, Ready, Started, Finished
    }

    private void OnEnable()
    {
        EventManager.LevelLoadedEvent.AddListener(GetReady);
    }
    private void OnDisable()
    {
        EventManager.LevelLoadedEvent.RemoveListener(GetReady);
    }

    void GetReady(LevelLoadedEventData _data)
    {
        Debug.Log("Game Ready!");
        _gameState = GameStates.Ready;
    }

    public void StartGame()
    {
        Debug.Log("Start Game!");
        _gameState = GameStates.Started;

        EventManager.LevelStartEvent?.Invoke();

        SpawnController.Instance.SpawnBats(true);
        SpawnController.Instance.SpawnRock(true);
    }

    public void FinishLevel(bool _success)
    {
        SpawnController.Instance.SpawnBats(false);
        SpawnController.Instance.SpawnRock(false);

        _gameState = GameStates.Finished;

        if (_success)
        {
            EventManager.LevelSuccessEvent?.Invoke();
        }
        else
        {
            EventManager.LevelFailEvent?.Invoke();
        }
    }





}
