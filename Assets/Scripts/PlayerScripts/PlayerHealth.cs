using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;
    public HealthBar healthBar;
    public static event Action OnPlayerDeath;
    public int maxLives = 10;
    private int lives;

    [Header("--Respawn Points--")]

    public Transform[] respawnPoints;
    public LoseMenu loseMenu;

    [Header("--Images--")]

    public Image[] heartImages;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
       health = maxHealth;
       healthBar.SetMaxHealth(maxHealth);

       lives = PlayerPrefs.GetInt("PlayerLives", 3);
       UpdateHeartsUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseMedkit();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            audioManager.PlaySound(audioManager.dying);
            Die();
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        healthBar.SetHealth(maxHealth);
        gameObject.SetActive(true);
    }

    public void ResetLives()
    {
        lives = 3;
        PlayerPrefs.SetInt("PlayerLives", lives);
        PlayerPrefs.Save();
        UpdateHeartsUI();
    }

    public void AddLife()
    {
        if (lives < maxLives)
        {
            lives++;
            PlayerPrefs.SetInt("PlayerLives", lives);
            PlayerPrefs.Save();
            UpdateHeartsUI();
        }
    }


    public int GetLives()
    {
        return lives;
    }

    public void UseMedkit()
    {
        if (Shop.Instance.medkitCount > 0)
        {
            health = maxHealth;
            healthBar.SetHealth(health);
            Shop.Instance.medkitCount--;
            PlayerPrefs.SetInt("MedkitCount", Shop.Instance.medkitCount);
            PlayerPrefs.Save();
            Shop.Instance.UpdateMedkitUI();
        }
    }

    private void Die()
    {
        lives--;
        PlayerPrefs.SetInt("PlayerLives", lives);
        PlayerPrefs.Save();
        UpdateHeartsUI();

        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            gameObject.SetActive(false);
            OnPlayerDeath?.Invoke();
            PlayerPrefs.SetInt("GameOver", 1);
            PlayerPrefs.Save();
            loseMenu.ShowLoseMenu();
        }
    }

    private void Respawn()
    {
        health = maxHealth;
        healthBar.SetHealth(maxHealth);

        Transform randomSpawnPoint = respawnPoints[UnityEngine.Random.Range(0, respawnPoints.Length)];
        transform.position = randomSpawnPoint.position;

        gameObject.SetActive(true);
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < lives;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(2);
        }
    }
}
