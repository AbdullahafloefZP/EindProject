using UnityEngine;
using UnityEngine.UI;

public class StatisticsUI : MonoBehaviour
{
    public Text enemiesKilledText;
    public Text moneyCollectedText;
    public Text highestLevelText;
    public Text highestWaveText;

    private void Start()
    {
        UpdateStatisticsUI();
    }

    public void UpdateStatisticsUI()
    {
        enemiesKilledText.text = "Enemies Killed: " + StatisticsManager.Instance.GetEnemiesKilled();
        moneyCollectedText.text = "Money Collected: " + StatisticsManager.Instance.GetMoneyCollected();
        highestLevelText.text = "Highest Level: " + StatisticsManager.Instance.GetHighestLevel();
        highestWaveText.text = "Highest Wave: " + StatisticsManager.Instance.GetHighestWave();
    }
}