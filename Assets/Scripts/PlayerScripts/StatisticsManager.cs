using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance { get; private set; }
    private StatisticsUI statisticsUI;

    private int enemiesKilled;
    private int moneyCollected;
    private int highestLevel;
    private int highestWave;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadStatistics();
    }

    private void Start()
    {
        statisticsUI = FindObjectOfType<StatisticsUI>();
    }

    public void IncrementEnemiesKilled()
    {
        enemiesKilled++;
        SaveStatistics();
        UpdateUI();
    }

    public void IncrementMoneyCollected(int amount)
    {
        moneyCollected += amount;
        SaveStatistics();
        UpdateUI();
    }

    public void UpdateHighestLevel(int level)
    {
        if (level > highestLevel)
        {
            highestLevel = level;
            SaveStatistics();
            UpdateUI();
        }
    }

    public void UpdateHighestWave(int wave)
    {
        if (wave > highestWave)
        {
            highestWave = wave;
            SaveStatistics();
            UpdateUI();
        }
    }

    public void ResetStatistics()
    {
        enemiesKilled = 0;
        moneyCollected = 0;
        highestLevel = 0;
        highestWave = 0;
        SaveStatistics();
        UpdateUI();
    }

    private void SaveStatistics()
    {
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetInt("MoneyCollected", moneyCollected);
        PlayerPrefs.SetInt("HighestLevel", highestLevel);
        PlayerPrefs.SetInt("HighestWave", highestWave);
        PlayerPrefs.Save();
    }

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

    public int GetEnemiesKilled() => enemiesKilled;
    public int GetMoneyCollected() => moneyCollected;
    public int GetHighestLevel() => highestLevel;
    public int GetHighestWave() => highestWave;
}
