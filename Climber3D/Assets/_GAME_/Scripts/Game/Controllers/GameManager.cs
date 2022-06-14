using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : PersistentSingleton<GameManager>
{

    #region Locals
    GameStates _gameState = GameStates.Loading;
    PlayerController _player;
    #endregion

    #region Properties
    public GameStates GameState => _gameState;
    public PlayerController Player { get { return _player; } set { _player = value; } }
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

    }

    public void StartGame()
    {

    }

    public void FinishLevel(bool _success)
    {

    }





}
