using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;
    public HealthBar healthBar; // Referentie naar de HealthBar script
    public static event Action OnPlayerDeath; // Event voor speler dood
    public int maxLives = 10;
    private int lives;

    [Header("--Respawn Points--")]
    public Transform[] respawnPoints; // Array van respawn points
    public LoseMenu loseMenu; // Referentie naar het verliesmenu script

    [Header("--Images--")]
    public Image[] heartImages; // Array van hartafbeeldingen voor levensweergave
    AudioManager audioManager; // Referentie naar de AudioManager component

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
       health = maxHealth; // Zet de gezondheid op maximaal
       healthBar.SetMaxHealth(maxHealth); // Stel de HealthBar in

       lives = PlayerPrefs.GetInt("PlayerLives", 3); // pak het aantal levens van playerprefs
       UpdateHeartsUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseMedkit(); // Gebruik een medkit als de Q-toets is ingedrukt
        }
    }

    // Methode om de speler te damagen
    public void TakeDamage(int damage)
    {
        health -= damage; // Verminder de gezondheid met de damage
        healthBar.SetHealth(health); // Update de HealthBar

        if (health <= 0)
        {
            audioManager.PlaySound(audioManager.dying); // Speel sterfgeluid af
            Die();
        }
    }

    // Methode om de gezondheid te resetten
    public void ResetHealth()
    {
        health = maxHealth; // Zet de gezondheid op maximaal
        healthBar.SetHealth(maxHealth); // Update de HealthBar
        gameObject.SetActive(true); // Maak het spelobject weer actief
    }

    // Methode om het aantal levens te resetten
    public void ResetLives()
    {
        lives = 3; // Zet het aantal levens op 3
        PlayerPrefs.SetInt("PlayerLives", lives); // Sla het aantal levens op in PlayerPrefs
        PlayerPrefs.Save();
        UpdateHeartsUI();
    }

    // Methode om een leven toe te voegen
    public void AddLife()
    {
        if (lives < maxLives)
        {
            lives++; // Verhoog het aantal levens
            PlayerPrefs.SetInt("PlayerLives", lives); // Sla het aantal levens op in PlayerPrefs
            PlayerPrefs.Save();
            UpdateHeartsUI();
        }
    }

    // Methode om het huidige aantal levens op te halen
    public int GetLives()
    {
        return lives;
    }

    // Methode om een medkit te gebruiken
    public void UseMedkit()
    {
        if (Shop.Instance.medkitCount > 0)
        {
            health = maxHealth; // Zet de gezondheid op maximaal
            healthBar.SetHealth(health); // zet de healthbar op het maximum
            Shop.Instance.medkitCount--; // Verminder het aantal medkits
            PlayerPrefs.SetInt("MedkitCount", Shop.Instance.medkitCount); // Sla het aantal medkits op in PlayerPrefs
            PlayerPrefs.Save();
            Shop.Instance.UpdateMedkitUI(); // Update de UI voor medkits
        }
    }

    // Methode die wordt aangeroepen wanneer de speler sterft
    private void Die()
    {
        lives--; // Verminder het aantal levens
        PlayerPrefs.SetInt("PlayerLives", lives); // Sla het aantal levens op in PlayerPrefs
        PlayerPrefs.Save();
        UpdateHeartsUI();

        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            gameObject.SetActive(false); // Maak de speler character inactief
            OnPlayerDeath?.Invoke(); // Roep het OnPlayerDeath event aan
            PlayerPrefs.SetInt("GameOver", 1); // Sla de game over status op in PlayerPrefs
            PlayerPrefs.Save();
            loseMenu.ShowLoseMenu(); // Toon het verliesmenu
        }
    }

    // Methode om de speler te respawnen
    private void Respawn()
    {
        health = maxHealth; // Zet de gezondheid op maximaal
        healthBar.SetHealth(maxHealth); // Update de HealthBar

        Transform randomSpawnPoint = respawnPoints[UnityEngine.Random.Range(0, respawnPoints.Length)]; // Kies een willekeurig spawnpunt
        transform.position = randomSpawnPoint.position; // Zet de positie van de speler op het spawnpunt

        gameObject.SetActive(true); // Maak het player character weer actief
    }

    // Methode om de UI van de hartafbeeldingen bij te werken
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < lives; // Schakel hartafbeeldingen in of uit op basis van het aantal levens
        }
    }

    // Methode die wordt aangeroepen wanneer er een collision is met de enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(2); // damage de speler bij botsing met een vijand
        }
    }

    // Methode om het aantal levens op te slaan
    public void SaveLives()
    {
        PlayerPrefs.SetInt("PlayerLives", lives); // Sla het aantal levens op in PlayerPrefs
        PlayerPrefs.Save();
    }

    // Methode om het aantal levens te laden
    public void LoadLives()
    {
        lives = PlayerPrefs.GetInt("PlayerLives", maxLives); // Laad het aantal levens uit PlayerPrefs
        UpdateHeartsUI();
    }
}