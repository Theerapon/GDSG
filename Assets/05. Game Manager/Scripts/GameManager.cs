﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSE,
    }

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set { _currentGameState = value; }
    }

    GameState _currentGameState = GameState.PREGAME;
    GameState _previousGameState;
    string _currentLevelName = string.Empty;

    public GameObject[] SystemPrefabs;
    List<GameObject> _instancedSystemPrefabs;

    public Events.EventGameState OnGameStateChanged;
    

    private void Start()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuLoadComplete.AddListener(HandleMainMenuLoadComplete);
    }

    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }
    }

    private void HandleMainMenuLoadComplete(bool loadGame)
    {
        if (!loadGame)
        {
            // UnloadLevel(_currentLevelName);
        }
    }

    void UpdateState(GameState state)
    {
        _previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;
            case GameState.PAUSE:
                Time.timeScale = 1.0f;
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(_currentGameState, _previousGameState);
    }

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_currentLevelName == "Main")
        {
            UpdateState(GameState.RUNNING);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;

        _currentLevelName = levelName;
    }

    protected void OnDestroy()
    {
        if (_instancedSystemPrefabs == null)
            return;

        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void PuaseGame()
    {
        if (_currentGameState == GameState.RUNNING)
        {
            UpdateState(GameState.PAUSE);
        }
        else if(_currentGameState == GameState.PAUSE && _previousGameState == GameState.RUNNING)
        {
            UpdateState(GameState.RUNNING);
        }

    }


    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }
}
