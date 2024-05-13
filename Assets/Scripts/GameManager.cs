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
        ShowMainMenu();
        CheckSavedGame();
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameplayComponents.SetActive(false);
    }

    private void CheckSavedGame()
    {
        if (PlayerPrefs.HasKey("CurrentWaveNumber"))
        {
            playGameButton.onClick.RemoveAllListeners();
            playGameButton.onClick.AddListener(StartNewGame);
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ContinueGame);
        }
        else
        {
            playGameButton.onClick.RemoveAllListeners();
            playGameButton.onClick.AddListener(StartNewGame);
            continueButton.gameObject.SetActive(false);
        }
    }

    public void StartNewGame()
    {
        ResetGameData();
        ClearCoins();
        mainMenuCanvas.SetActive(false);
        gameplayComponents.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void ContinueGame()
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

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
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
