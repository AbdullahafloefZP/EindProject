using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : MonoBehaviour
{
    public static bool PlayerHasDied = false;
    public GameObject loseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public Shop shop;
    public LevelSystem levelSystem;
    public WaveSpawner waveSpawner;
    public Canvas canvasToDisable;
    public Button continueButton;
    public GameObject pauseMenuUI;
    private StatisticsUI statisticsUI;
    public PauseMenu pauseMenu;

    void Start()
    {
        loseMenuUI.SetActive(false);

        if (PlayerPrefs.GetInt("GameOver", 0) == 1)
        {
            continueButton.interactable = false;
        }

        statisticsUI = loseMenuUI.GetComponentInChildren<StatisticsUI>();
    }

    void Update()
    {
        if (PlayerHasDied)
        {
            ShowLoseMenu();
            PlayerHasDied = false;
        }
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += ShowLoseMenu;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= ShowLoseMenu;
    }

    public void ShowLoseMenu()
    {
        pauseMenuUI.SetActive(false);
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(false);
        }

        continueButton.interactable = false;

        if (statisticsUI != null)
        {
            statisticsUI.UpdateStatisticsUI();
        }

        PauseMenu.GameIsPaused = false;
    }

    public void Retry()
    {
        ClearCoins();
        ResetGameData();
    }

    private void ResetGameData()
    {
        playerMovement.ResetPosition();
        playerHealth.ResetHealth();
        playerHealth.ResetLives();
        shop.ResetMoneyAndWeapons();
        levelSystem.ResetLevel();
        waveSpawner.ResetWaveProgression();

        PlayerPrefs.SetInt("GameOver", 0);
        PlayerPrefs.SetInt("PlayerLives", playerHealth.maxLives);

        PlayerPrefs.Save();

        Time.timeScale = 1f;
        loseMenuUI.SetActive(false);
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(true);
        }

        PlayerShoot[] playerShoots = FindObjectsOfType<PlayerShoot>();
        foreach (var playerShoot in playerShoots)
        {
            playerShoot.ResetReloadingState();
        }

        FindObjectOfType<GameManager>().CheckSavedGame();
    }

    public void QuitGame()
    {
        Application.Quit();
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