using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    [Header("Data")]
    private Vector3 lastCheckpointPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        Checkpoint.onInteracted += CheckPointInteractedCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }
    private void OnDestroy()
    {
        Checkpoint.onInteracted -= CheckPointInteractedCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckPointInteractedCallback(Checkpoint checkpoint)
    {
        lastCheckpointPosition = checkpoint.GetPosition();
    }

    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
           
                case GameState.LevelComplete:
                lastCheckpointPosition = Vector3.zero;
                break;

        }
    }
}
