using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;
    public Text moneyText;
    public static int moneyAmount;
    public WeaponHolder weaponHolder;
    public delegate void OnMoneyChanged();
    public static event OnMoneyChanged MoneyChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount", 0);
        UpdateMoneyDisplay();
    }

    public void UpdateMoneyDisplay()
    {
        moneyText.text = moneyAmount.ToString();
        MoneyChanged?.Invoke();
    }

    public void ChangeMoney(int amount)
    {
        moneyAmount += amount;
        PlayerPrefs.SetInt("MoneyAmount", moneyAmount);
        UpdateMoneyDisplay();
    }
}