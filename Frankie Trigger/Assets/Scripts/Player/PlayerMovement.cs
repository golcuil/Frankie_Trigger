using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System;

public class PlayerMovement : MonoBehaviour
{
    enum State { Idle, Run, Warzone, Dead}


    [Header("Elements")]
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private CharakterIK playerIK;
    [SerializeField] private CharacterRagdoll characterRagdoll;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _slowMoScale;
    [SerializeField] private Transform enemyTarget;
    private State state;
    private Warzone currentWarzone;

    [Header("Spline Settings")]
    private float _warzoneTimer;

    [Header("Actions")]
    public static Action onEnteredWarzone;
    public static Action onExitedWarzone;
    public static Action onDied;


    private void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangeCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangeCallback;
    }
    void Start()
    {
        state = State.Idle;

        //Place the player at the last position (if any)
        transform.position = CheckpointManager.Instance.GetCheckpointPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsGameState())
            ManageState();
    }

    private void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                StartRunning();
                break;

        }
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Run:
                Move();
                break;
            case State.Warzone:
                ManageWarzoneState();
                break;
        }
    }

    private void StartRunning()
    {
        state = State.Run;
        _playerAnimator.PlayRunAnimation();
    }

    private void Move()
    {
        transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
    }

    public void EnteredWarzoneCallback(Warzone warzone)
    {
        if (currentWarzone != null)
            return;

        state = State.Warzone;
        currentWarzone = warzone;

        currentWarzone.StartAnimatingIKTarget();

        _warzoneTimer = 0;

        _playerAnimator.Play(currentWarzone.GetAnimationToPlay(), currentWarzone.GetAnimatorSpeed());

        Time.timeScale = _slowMoScale;
        Time.fixedDeltaTime = _slowMoScale / 50;

        playerIK.ConfigureIK(currentWarzone.GetIKTarget());

        onEnteredWarzone?.Invoke();

    }

    private void ManageWarzoneState()
    {
        _warzoneTimer += Time.deltaTime;

        float splinePercent = Mathf.Clamp01(_warzoneTimer / currentWarzone.GetDuration());
        transform.position = currentWarzone.GetPlayerSpline().EvaluatePosition(splinePercent);

        if (splinePercent >= 1)
            TryExitWarzone();
    }

    private void TryExitWarzone()
    {
        Warzone nextWarzone = currentWarzone.GetNextWarzone();

        if (nextWarzone == null)
            ExitWarzone();
        else
        {
            currentWarzone = null;
            EnteredWarzoneCallback(nextWarzone);
        }
            
        
    }

    private void ExitWarzone()
    {
        currentWarzone = null;

        state = State.Run;
        _playerAnimator.Play("Run", 1f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f / 50;

        playerIK.DisableIK();
        onExitedWarzone?.Invoke();
    }

    public Transform GetEnemyTarget()
    {
        return enemyTarget;
    }

    public void TakeDamage()
    {
        state = State.Dead;
        
        characterRagdoll.Ragdollify();

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f / 50;

        onDied?.Invoke();
        GameManager.Instance.SetGameState(GameState.GameOver);

    }

    public void HitFinishLine()
    {
        state = State.Idle;
        _playerAnimator.PlayIdleAnimation();

        GameManager.Instance.SetGameState(GameState.LevelComplete);
    }

    public Warzone GetCurrentWarzone()
    {
        return currentWarzone;
    }
}
