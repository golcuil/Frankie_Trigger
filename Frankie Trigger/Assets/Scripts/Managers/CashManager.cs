using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance;

    [Header("Data")]
    private int coins;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI[] coinTexts;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddCoins(500);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveData();
    }

    public bool CanPurchase(int amount)
    {
        return coins >= amount;
    }

    public void Purchase(int amount)
    {
       
        coins -= amount;
        SaveData();
    }

    private void UpdateTexts()
    {
        foreach (var coinText in coinTexts)
        {
            coinText.text = coins.ToString();
        }
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("Coins", 100);
        UpdateTexts();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Coins", coins);
        UpdateTexts();
    }
}
