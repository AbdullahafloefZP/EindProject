using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    // Singleton instantie van StatisticsManager
    public static StatisticsManager Instance { get; private set; }
    // Verwijzing naar de Statsitieken script
    private StatisticsUI statisticsUI;

    // Variabelen om verschillende statistieken bij te houden
    private int enemiesKilled;
    private int moneyCollected;
    private int highestLevel;
    private int highestWave;

    // Deze functie wordt aangeroepen wanneer het object wordt geïnitialiseerd
    private void Awake()
    {
        // Controleer of er al een instantie van StatisticsManager bestaat
        if (Instance == null)
        {
            Instance = this; // Stel deze instantie in als de Singleton
            DontDestroyOnLoad(gameObject); // Voorkom dat dit object wordt vernietigd bij het laden van een nieuwe scène
        }
        else
        {
            Destroy(gameObject); // Vernietig deze gameobject als er al een instance bestaat
            return; // Stop verdere uitvoering
        }

        LoadStatistics();
    }

    // Deze functie wordt aangeroepen bij het starten van het spel
    private void Start()
    {
        statisticsUI = FindObjectOfType<StatisticsUI>();
    }

    // Verhoog het aantal gedode vijanden met 1
    public void IncrementEnemiesKilled()
    {
        enemiesKilled++;
        SaveStatistics();
        UpdateUI();
    }

    // Verhoog het verzamelde geld met een bepaald bedrag
    public void IncrementMoneyCollected(int amount)
    {
        moneyCollected += amount;
        SaveStatistics();
        UpdateUI();
    }

    // Update het hoogste niveau als het nieuwe niveau hoger is dan het huidige hoogste niveau
    public void UpdateHighestLevel(int level)
    {
        if (level > highestLevel)
        {
            highestLevel = level;
            SaveStatistics();
            UpdateUI();
        }
    }

    // Update de hoogste golf als de nieuwe golf hoger is dan de huidige hoogste golf
    public void UpdateHighestWave(int wave)
    {
        if (wave > highestWave)
        {
            highestWave = wave;
            SaveStatistics();
            UpdateUI();
        }
    }

    // Reset alle statistieken
    public void ResetStatistics()
    {
        enemiesKilled = 0;
        moneyCollected = 0;
        highestLevel = 0;
        highestWave = 0;

        SaveStatistics(); // Sla de statistieken op
        UpdateUI(); // Update de UI
    }

    // Sla de statistieken op met behulp van PlayerPrefs
    private void SaveStatistics()
    {
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetInt("MoneyCollected", moneyCollected);
        PlayerPrefs.SetInt("HighestLevel", highestLevel);
        PlayerPrefs.SetInt("HighestWave", highestWave);
        PlayerPrefs.Save(); // Zorg ervoor dat de gegevens worden opgeslagen
    }

    // Laad de statistieken van PlayerPrefs
    private void LoadStatistics()
    {
        enemiesKilled = PlayerPrefs.GetInt("EnemiesKilled", 0);
        moneyCollected = PlayerPrefs.GetInt("MoneyCollected", 0);
        highestLevel = PlayerPrefs.GetInt("HighestLevel", 0);
        highestWave = PlayerPrefs.GetInt("HighestWave", 0);
    }

    private void UpdateUI()
    {
        if (statisticsUI != null)
        {
            statisticsUI.UpdateStatisticsUI();
        }
    }

    // Publieke methoden om de statistieken op te halen
    public int GetEnemiesKilled() => enemiesKilled;
    public int GetMoneyCollected() => moneyCollected;
    public int GetHighestLevel() => highestLevel;
    public int GetHighestWave() => highestWave;
}