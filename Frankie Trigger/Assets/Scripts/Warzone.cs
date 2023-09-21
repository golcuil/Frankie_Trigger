using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Warzone : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SplineContainer playerSpline;
    [SerializeField] Transform ikTarget;
    [SerializeField] private SplineAnimate ikSplineAnimate;

    [Header("Settings")]
    [SerializeField] private float duration;
    [SerializeField] private float animatorSpeed;
    [SerializeField] private string animationToPlay;
    // Start is called before the first frame update
    void Start()
    {
        ikSplineAnimate.Duration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimatingIKTarget()
    {
        ikSplineAnimate.Play();
    }

    public Spline GetPlayerSpline()
    {
        return playerSpline.Spline;
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
