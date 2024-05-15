using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameplayComponents;
    public Button playGameButton;
    public Button continueButton;
    public GameObject pauseMenuCanvas;

    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public Shop shop;
    public LevelSystem levelSystem;
    public WaveSpawner waveSpawner;

    private void Start()
    {
        SetupButtonListeners();
        ShowMainMenu();
        CheckSavedGame();
    }

    private void SetupButtonListeners()
    {
        playGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameplayComponents.SetActive(false);
    }

    private void CheckSavedGame()
    {
        continueButton.gameObject.SetActive(PlayerPrefs.HasKey("CurrentWaveNumber"));
    }

    public void StartNewGame()
    {
        ResetGameData();
        ClearCoins();
        ShowGameUI();
    }

    public void ContinueGame()
    {
        ShowGameUI();
    }

    private void ShowGameUI()
    {
        mainMenuCanvas.SetActive(false);
        gameplayComponents.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGameData()
    {
        playerMovement.ResetPosition();
        playerHealth.ResetHealth();
        shop.ResetMoneyAndWeapons();
        levelSystem.ResetLevel();
        waveSpawner.ResetWaveProgression();
    }

    private void ClearCoins()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        foreach (Coin coin in coins)
        {
            Destroy(coin.gameObject);
        }
    }
}