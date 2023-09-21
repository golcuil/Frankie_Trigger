using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{
    enum State { Idle, Run, Warzone}


    [Header("Elements")]
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerIK playerIK;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _slowMoScale;
    private State _state;
    private Warzone currentWarzone;

    [Header("Spline Settings")]
    private float _warzoneTimer;

    void Start()
    {
        _state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRunning();
        }
        ManageState();
    }

    private void ManageState()
    {
        switch (_state)
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
        _state = State.Run;
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

        _state = State.Warzone;
        currentWarzone = warzone;

        currentWarzone.StartAnimatingIKTarget();

        _warzoneTimer = 0;

        _playerAnimator.Play(currentWarzone.GetAnimationToPlay(), currentWarzone.GetAnimatorSpeed());

        Time.timeScale = _slowMoScale;

        playerIK.ConfigureIK(currentWarzone.GetIKTarget());

    }

    private void ManageWarzoneState()
    {
        _warzoneTimer += Time.deltaTime;

        float splinePercent = _warzoneTimer / currentWarzone.GetDuration();
        transform.position = currentWarzone.GetPlayerSpline().EvaluatePosition(splinePercent);

        if (_warzoneTimer >= 1)
            ExitWarzone();
    }

    private void ExitWarzone()
    {
        _state = State.Run;
        currentWarzone = null;
        _playerAnimator.Play("Run", 1f);

        Time.timeScale = 1f;

        playerIK.DisableIK();
    }
}
