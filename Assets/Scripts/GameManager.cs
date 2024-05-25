using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Verwijzingen naar de UI componenten en gameobjecten
    public GameObject mainMenuCanvas;
    public GameObject gameplayComponents;
    public Button playGameButton;
    public Button continueButton;
    public GameObject pauseMenuCanvas;

    // Verwijzingen naar de scripts en geluid
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public Shop shop;
    public LevelSystem levelSystem;
    public WaveSpawner waveSpawner;
    public AudioSource backgroundSound;
    public AudioClip background2;

    // Deze functie wordt aangeroepen bij het starten van het spel
    private void Start()
    {
        SetupButtonListeners();
        ShowMainMenu();
        CheckSavedGame();
    }

    private void SetupButtonListeners()
    {
        playGameButton.onClick.AddListener(StartNewGame); // check of er geklikt word op de playGameButton en roep de StartNewGame functie dan
        continueButton.onClick.AddListener(ContinueGame); // check of er geklikt word op de continueButton en roep de ContinueGame functie dan
    }

    // Functie om het hoofdmenu weer te geven
    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true); // Toon het hoofdmenu canvas
        gameplayComponents.SetActive(false); // Verberg de gameplay componenten of verberg de game gewoon
    }

    // Functie om te controleren of er een opgeslagen spel is
    public void CheckSavedGame()
    {
        if (PlayerPrefs.GetInt("GameOver", 0) == 1)
        {
            continueButton.gameObject.SetActive(false); // Verberg de continue knop als de speler verloren is
        }
        else
        {
            continueButton.gameObject.SetActive(PlayerPrefs.HasKey("CurrentWaveNumber")); // Toon de continue knop als er een golf opgeslagen word
        }
    }

    // Functie om een nieuw spel te starten
    public void StartNewGame()
    {
        PlayerPrefs.SetInt("GameOver", 0); // Zet de GameOver status op 0
        PlayerPrefs.SetInt("PlayerLives", playerHealth.maxLives); // Sla het maximale aantal levens op
        PlayerPrefs.Save(); // Sla alles op in playerprefs

        ResetGameData();
        ClearCoins();
        ShowGameUI();
    }

    // Functie om een bestaand spel voort te zetten
    public void ContinueGame()
    {
        playerHealth.LoadLives(); // Laad het aantal levens van de speler
        ShowGameUI(); // Toon de gameplay UI
    }

    // Functie om de gameplay UI weer te geven
    private void ShowGameUI()
    {
        mainMenuCanvas.SetActive(false); // Verberg het hoofdmenu canvas
        gameplayComponents.SetActive(true); // Toon de gameplay componenten
        pauseMenuCanvas.SetActive(false); // Verberg het pauzemenu canvas
        Time.timeScale = 1; // Zet de tijdschaal op 1 om het spel te hervatten

        backgroundSound.clip = background2; // Zet de achtergrondmuziek naar background2
        backgroundSound.Play(); // Speel de achtergrondmuziek af
    }

    // Functie om het spel te verlaten
    public void QuitGame()
    {
        Application.Quit(); // Sluit de applicatie
    }

    // Functie om alle speldata te resetten
    private void ResetGameData()
    {
        playerMovement.ResetPosition(); // Reset de positie van de speler
        playerHealth.ResetHealth(); // Reset de gezondheid van de speler
        playerHealth.ResetLives(); // Reset het aantal levens van de speler
        shop.ResetMoneyAndWeapons(); // Reset het geld en de wapens in de winkel
        levelSystem.ResetLevel(); // Reset het levelsysteem
        waveSpawner.ResetWaveProgression(); // Reset de golfvoortgang
        
        // Reset de herlaadstatus van alle wapens
        PlayerShoot[] playerShoots = FindObjectsOfType<PlayerShoot>();
        foreach (var playerShoot in playerShoots)
        {
            playerShoot.ResetReloadingState();
        }
    }

    // Functie om alle munten in de scene te verwijderen
    private void ClearCoins()
    {
        Coin[] coins = FindObjectsOfType<Coin>();
        foreach (Coin coin in coins)
        {
            Destroy(coin.gameObject); // Vernietig elke munt
        }
    }
}