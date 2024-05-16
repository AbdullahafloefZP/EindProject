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
    
    void Start()
    {
        loseMenuUI.SetActive(false);

        if (PlayerPrefs.GetInt("GameOver", 0) == 1)
        {
            continueButton.interactable = false;
        }
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
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(false);
        }

        continueButton.interactable = false;
    }

    public void Retry()
    {
        playerMovement.ResetPosition();
        playerHealth.ResetHealth();
        playerHealth.ResetLives();
        shop.ResetMoneyAndWeapons();
        levelSystem.ResetLevel();
        waveSpawner.ResetWaveProgression();

        PlayerShoot[] playerShoots = FindObjectsOfType<PlayerShoot>();
        foreach (var playerShoot in playerShoots)
        {
            playerShoot.ResetReloadingState();
        }

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("GameOver", 0);
        PlayerPrefs.SetInt("PlayerLives", playerHealth.maxLives);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        loseMenuUI.SetActive(false);
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(true);
        }

        FindObjectOfType<GameManager>().CheckSavedGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}