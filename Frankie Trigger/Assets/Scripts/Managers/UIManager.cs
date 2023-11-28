using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject outfitShopPanel;
    [SerializeField] private GameObject missionPanel;
    // Start is called before the first frame update

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(false);
                gameOverPanel.SetActive(false);

                CloseOutfitShop();
                CloseMissionPanel();
                break;

            case GameState.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                break;

            case GameState.GameOver:
                gamePanel.SetActive(false);
                gameOverPanel.SetActive(true);
                break;

            case GameState.LevelComplete:
                gamePanel.SetActive(false);
                levelCompletePanel.SetActive(true);
                break;

        }
    }

    public void PlayButtonCallback()
    {
        GameManager.Instance.SetGameState(GameState.Game);
        
    }

    public void RetryButtonCallback()
    {
        GameManager.Instance.Retry();
    }

    public void NextButtonCallback()
    {
        GameManager.Instance.NextLevel();
    }

    public void OpenOutfitShop()
    {
        outfitShopPanel.SetActive(true);
    }

    public void CloseOutfitShop()
    {
        outfitShopPanel.SetActive(false);
    }

    public void OpenMissionPanel()
    {
        missionPanel.SetActive(true);
    }

    public void CloseMissionPanel()
    {
        missionPanel.SetActive(false);
    }
}
