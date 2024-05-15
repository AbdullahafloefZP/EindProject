using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    public static bool PlayerHasDied = false;
    public GameObject loseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public Shop shop;
    public LevelSystem levelSystem;
    public WaveSpawner waveSpawner;

    void Start()
    {
        loseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (PlayerHasDied)
        {
            Lose();
            PlayerHasDied = false;
        }
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += Lose;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= Lose;
    }

    public void Lose()
    {
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        ClearCoins();
        playerMovement.ResetPosition();
        playerHealth.ResetHealth();
        shop.ResetMoneyAndWeapons();
        levelSystem.ResetLevel();
        waveSpawner.ResetWaveProgression();

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        loseMenuUI.SetActive(false);
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
