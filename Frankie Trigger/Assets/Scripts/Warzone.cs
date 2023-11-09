using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Dreamteck.Splines;

public class Warzone : MonoBehaviour
{
    [Header("Elements")]
    //[SerializeField] private SplineContainer playerSpline; old spline
    [SerializeField] Transform ikTarget;
    //[SerializeField] private SplineAnimate ikSplineAnimate; old spline
    [SerializeField] private SplineComputer newPlayerSpline;
    [SerializeField] private SplineFollower ikSplineFollower;

    [Header("Settings")]
    [SerializeField] private float duration;
    [SerializeField] private float animatorSpeed;
    [SerializeField] private string animationToPlay;
    // Start is called before the first frame update
    void Start()
    {
        //ikSplineAnimate.Duration = duration; old spline
        ikSplineFollower.followDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimatingIKTarget()
    {
        //ikSplineAnimate.Play(); old spline
        ikSplineFollower.follow = true;
    }

    public SplineComputer GetPlayerSpline()
    {
        return newPlayerSpline;
    }

    public float GetDuration()
    {
        return duration;
    }

    public string GetAnimationToPlay()
    {
        return animationToPlay;
    }

    public float GetAnimatorSpeed()
    {
        return animatorSpeed;
    }

    public Transform GetIKTarget()
    {
        return ikTarget;
    }
}
