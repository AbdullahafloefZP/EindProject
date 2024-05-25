using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LoseMenu : MonoBehaviour
{
    // Statische boolean om bij te houden of de speler is overleden
    public static bool PlayerHasDied = false;
    // Verwijzing naar het verliesmenu UI object
    public GameObject loseMenuUI;
    // Verwijzing naar de PlayerMovement script
    public PlayerMovement playerMovement;
    // Verwijzing naar de PlayerHealth script
    public PlayerHealth playerHealth;
    // Verwijzing naar de Shop script
    public Shop shop;
    // Verwijzing naar het LevelSystem script
    public LevelSystem levelSystem;
    // Verwijzing naar de WaveSpawner script
    public WaveSpawner waveSpawner;
    // Verwijzing naar een canvas (de canvas waar al de dingen op staan, de health, levens, minimap, medkit en ammo enzo)
    public Canvas canvasToDisable;
    // Verwijzing naar de continue knop
    public Button continueButton;
    // Verwijzing naar het pauzemenu UI object
    public GameObject pauseMenuUI;
    // Verwijzing naar de StatisticsUI script
    private StatisticsUI statisticsUI;
    // Verwijzing naar de PauseMenu script
    public PauseMenu pauseMenu;
    // Verwijzing naar de PlayerInput object
    public PlayerInput playerInput;

    // Deze functie wordt aangeroepen bij het starten van het spel
    void Start()
    {
        loseMenuUI.SetActive(false); // Zorg ervoor dat het verliesmenu inactive is bij het starten

        // Controleer of het spel eerder was beëindigd en stel de continue knop in
        if (PlayerPrefs.GetInt("GameOver", 0) == 1)
        {
            continueButton.interactable = false;
        }

        // Haal de StatisticsUI script op van het verliesmenu
        statisticsUI = loseMenuUI.GetComponentInChildren<StatisticsUI>();
    }

    // Deze functie wordt elke frame aangeroepen
    void Update()
    {
        // Controleer of de speler is overleden
        if (PlayerHasDied)
        {
            ShowLoseMenu(); // Toon het verliesmenu
            PlayerHasDied = false; // zet playerHasDied status op false
        }
    }

    // Deze functie wordt aangeroepen wanneer het losemenu wordt ingeschakeld
    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += ShowLoseMenu; // Abonneer op de OnPlayerDeath gebeurtenis
    }

    // Deze functie wordt aangeroepen wanneer het losemenu wordt uitgeschakeld
    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= ShowLoseMenu; // Meld af van de OnPlayerDeath gebeurtenis
    }

    // Functie om het verliesmenu te tonen
    public void ShowLoseMenu()
    {
        pauseMenuUI.SetActive(false); // Verberg het pauzemenu
        loseMenuUI.SetActive(true); // Toon het verliesmenu
        Time.timeScale = 0f; // Zet de tijdschaal op 0 om het spel te pauzeren

        // Schakel het canvas uit
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(false);
        }

        continueButton.interactable = false; // Maak de continue knop niet interactief als de speler dood gaat

        // Update de statistieken in de UI
        if (statisticsUI != null)
        {
            statisticsUI.UpdateStatisticsUI();
        }

        PauseMenu.GameIsPaused = false; // Zet de pauzestatus op false
        playerInput.DeactivateInput(); // Deactiveer de spelerinvoer
    }

    // Functie om het spel opnieuw te proberen
    public void Retry()
    {
        ClearCoins();
        ResetGameData();
    }

    // Functie om alle speldata te resetten
    private void ResetGameData()
    {
        playerMovement.ResetPosition(); // Reset de positie van de speler
        playerHealth.ResetHealth(); // Reset de health van de speler
        playerHealth.ResetLives(); // Reset het aantal levens van de speler
        shop.ResetMoneyAndWeapons(); // Reset het geld en de wapens in de winkel
        levelSystem.ResetLevel(); // Reset het levelsysteem
        waveSpawner.ResetWaveProgression(); // Reset de golfvoortgang
        playerInput.ActivateInput(); // Activeer de spelerinvoer
        playerMovement.ResetMovement(); // Reset de beweging van de speler

        PlayerPrefs.SetInt("GameOver", 0); // sla de GameOver status op in PlayerPrefs
        PlayerPrefs.SetInt("PlayerLives", playerHealth.maxLives); // Sla het maximale aantal levens van de speler op

        PlayerPrefs.Save(); // Sla de wijzigingen op in PlayerPrefs

        Time.timeScale = 1f; // Zet de tijdschaal terug naar 1 om het spel te hervatten
        loseMenuUI.SetActive(false); // Verberg het verliesmenu
        if (canvasToDisable != null)
        {
            canvasToDisable.gameObject.SetActive(true); // Schakel het canvas weer in
        }

        // Reset de herlaadstatus van alle wapens
        PlayerShoot[] playerShoots = FindObjectsOfType<PlayerShoot>();
        foreach (var playerShoot in playerShoots)
        {
            playerShoot.ResetReloadingState();
        }

        FindObjectOfType<GameManager>().CheckSavedGame(); // Controleer de opgeslagen spelstatus
    }

    // Functie om het spel te verlaten
    public void QuitGame()
    {
        Application.Quit(); // Sluit de applicatie
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