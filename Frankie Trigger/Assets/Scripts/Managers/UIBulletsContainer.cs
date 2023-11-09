using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBulletsContainer : MonoBehaviour
{
    public static UIBulletsContainer Instance;

    [Header("Elements")]
    [SerializeField] private Transform bulletsParent;

    [Header("Settings")]
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    private int bulletsShot;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        activeColor = new Color(245, 207, 0,255);
        inactiveColor = Color.grey;

        PlayerShooter.onShot += OnShotCallback;
        PlayerMovement.onEnteredWarzone += EnteredWarzoneCallback;
        PlayerMovement.onExitedWarzone += ExitedWarzoneCallback;
    }

    private void OnDestroy()
    {
        PlayerShooter.onShot -= OnShotCallback;

        PlayerMovement.onEnteredWarzone -= EnteredWarzoneCallback;
        PlayerMovement.onExitedWarzone -= ExitedWarzoneCallback;
    }
    // Start is called before the first frame update
    void Start()
    {
        bulletsParent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void EnteredWarzoneCallback()
    {
        bulletsParent.gameObject.SetActive(true);
    }

    void ExitedWarzoneCallback()
    {
        bulletsParent.gameObject.SetActive(false);
        Reload();
    }

    void OnShotCallback()
    {
        //different ways
        //if (bulletsShot > bulletsParent.childCount)
        //    bulletsShot = bulletsParent.childCount;
        //bulletsShot = Mathf.Clamp(bulletsShot, 0, bulletsParent.childCount);
        bulletsShot++;

        bulletsShot = Mathf.Min(bulletsShot, bulletsParent.childCount);

        bulletsParent.GetChild(bulletsShot - 1).GetComponent<Image>().color = inactiveColor;
    }

    void Reload()
    {
        bulletsShot = 0;

        for (int i = 0; i < bulletsParent.childCount; i++)
            bulletsParent.GetChild(i).GetComponent<Image>().color = activeColor;
    }

    public bool CanShoot()
    {
        return bulletsShot < bulletsParent.childCount;
    }


}
