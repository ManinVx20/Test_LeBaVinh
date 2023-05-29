using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private enum State
    {
        GameWaited = 0,
        GameStarted = 1,
        GamePaused = 2,
        GameFinished = 3,
    }

    private State _state;
    private Player _player;

    private void Start()
    {
        WaitGame();
    }

    public bool IsGameWaited()
    {
        return _state == State.GameWaited;
    }

    public void WaitGame()
    {
        _state = State.GameWaited;

        LevelManager.Instance.ResetLevelIndex();

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenuCanvas>();
    }

    public void StartGame()
    {
        _state = State.GameStarted;

        LevelManager.Instance.LoadLevel(() =>
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<LoadingCanvas>();
        }, () =>
        {
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GameplayCanvas>();
            UIManager.Instance.OpenUI<ControlCanvas>();
        });

        if (_player == null)
        {
            _player = Instantiate(ResourceManager.Instance.PlayerPrefab);
        }
    }

    public void FinishGame(bool isWon)
    {
        if (isWon)
        {
            // Win
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<WinCanvas>();
        }
        else
        {
            // Lose
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<LoseCanvas>();
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1.0f;

        LevelManager.Instance.UnloadLevel();

        if (_player != null)
        {
            _player.Despawn();
        }

        WaitGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<PauseCanvas>();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;

        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GameplayCanvas>();
        UIManager.Instance.OpenUI<ControlCanvas>();
    }
}
