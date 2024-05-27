using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    // Singleton instance van GameControl
    public static GameControl Instance;
    // Tekstveld voor het weergeven van het geldbedrag
    public Text moneyText;
    // Statisch geldbedrag
    public static int moneyAmount;
    // Verwijzing naar de WeaponHolder script
    public WeaponHolder weaponHolder;
    // Verwijzing naar het LevelSystem script
    public LevelSystem levelSystem;
    // Delegate voor de MoneyChanged gebeurtenis
    public delegate void OnMoneyChanged();
    // Statische gebeurtenis voor wanneer het geldbedrag verandert
    public static event OnMoneyChanged MoneyChanged;

    // Deze functie wordt aangeroepen wanneer het object wordt geïnitialiseerd
    void Awake()
    {
        // Controleer of er al een instance van GameControl bestaat
        if (Instance == null)
        {
            Instance = this; // Stel deze instance in als de Singleton
            DontDestroyOnLoad(gameObject); // Voorkom dat dit object wordt vernietigd bij het laden van een nieuwe scene
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Vernietig deze gameobject als er al een instance bestaat
        }
    }

    // Deze functie wordt aangeroepen bij het starten van het spel
    void Start()
    {
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount", 0); // sla het bedrag op zodat de speler 0 geld heeft wanneer de game start
        UpdateMoneyDisplay(); // Update de weergave van het geldbedrag
    }

    // Deze functie wordt elke frame aangeroepen
    void Update()
    {
        // Controleer of de 'M' toets is ingedrukt
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddMoney(100); // Voeg 100 coins toe
            levelSystem.AddExperience(100); // Voeg 100 ervaring toe
        }
    }

    // Functie om de geldweergave bij te werken
    public void UpdateMoneyDisplay()
    {
        moneyText.text = moneyAmount.ToString(); // Update het tekstveld met het huidige geldbedrag
        MoneyChanged?.Invoke(); // Controleer of er abonnees zijn en roep het event aan
    }

    // Functie om het geldbedrag te veranderen met een specifiek bedrag
    public void ChangeMoney(int amount)
    {
        moneyAmount += amount; // Verhoog het geldbedrag met het opgegeven bedrag
        PlayerPrefs.SetInt("MoneyAmount", moneyAmount); // Sla het nieuwe geldbedrag op
        UpdateMoneyDisplay(); // Update de weergave van het geldbedrag
    }

    // Functie om geld toe te voegen
    public void AddMoney(int amount)
    {
        ChangeMoney(amount); // Voeg het opgegeven bedrag toe aan het geldbedrag
    }
}