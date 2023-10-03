using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum GameState {Menu, Game, LevelComplete, GameOver };

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    private GameState gameState;

    [Header("Actions")]
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetGameState(GameState.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void NextLevel()
    {
        //Tell the level manager to increase the level Index;
        SceneManager.LoadScene(0);
    }
}
