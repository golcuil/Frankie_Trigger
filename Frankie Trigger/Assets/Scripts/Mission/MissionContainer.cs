using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MissionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Button claimButton;
    private int key;

    [Header("Actions")]
    public static Action<int> onRewardClaimed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(string title, string coinAmount, float progress, int key)
    {
        this.key = key;

        titleText.text = title;
        coinText.text = coinAmount;
        progressBar.value = progress;

        CheckIfCanClaim(progress);
    }

    public void UpdateProgress(float value)
    {
        progressBar.value = value;

        CheckIfCanClaim(value);
    }

    private void CheckIfCanClaim(float progress)
    {
        if (progress >= 1)
        {
            claimButton.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(false);
        }
    }

    public void Claim()
    {
        //Save the state of mission;
        onRewardClaimed?.Invoke(key);
    }

    public int GetKey()
    {
        return key;
    }
}
