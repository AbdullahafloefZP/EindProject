using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("--weapon prices--")]
    public Text[] gunriflePrice; // Tekstobject voor wapenprijzen

    [Header("--Buy buttons--")]
    public Button[] buyButton; // Knoppen om wapens te kopen

    [Header("--Equip buttons--")]
    public Button[] equipButton; // Knoppen om wapens te equippen

    [Header("--Texts--")]
    public Text[] equippedText; // Tekstobject voor het weergeven van equipped status
    public Text[] levelRequirementTexts; // Tekstobject voor level requirements
    public Text moneyAmountText; // Tekstobject voor het weergeven van het huidige geldbedrag

    [Header("--Weapon Level--")]
    public int[] levelRequirements; // Array met level requirements voor wapens

    [Header("--Medkit stuff--")]
    public int medkitLevelRequirement; // Level requirements voor medkits
    public Text medkitLevelRequirementText; // Tekstobject voor medkit level requirements
    public Text medkitPriceText; // Tekstobject voor de prijs van medkits
    public Button buyMedkitButton; // Knop om een medkit te kopen
    public Text medkitCountText; // Tekstobject voor het aantal medkits

    [Header("--Magazine stuff--")]
    public Text magazinePriceText; // Tekstobject voor de prijs van magazijnen
    public Button buyMagazineButton; // Knop om een magazijn te kopen

    [Header("--Extra lives stuff--")]
    public Text lifePriceText; // Tekstobject voor de prijs van extra levens
    public Button buyLifeButton; // Knop om een extra leven te kopen
    public int lifeLevelRequirement; // Level requirements voor extra levens
    public Text lifeLevelRequirementText; // Tekstobject voor level requirements van extra levens
    private int equippedWeaponIndex = -1; // Index van het equipped wapen
    private bool[] isWeaponSold; // Array om bij te houden of wapen is verkocht
    [HideInInspector] public int medkitCount = 0; // Aantal medkits

    [Header("--refs--")]
    public PlayerHealth playerHealth; // Verwijzing naar de PlayerHealth script
    public GunChange gunChange; // Verwijzing naar de GunChange script
    public PlayerShoot[] playerShoots; // Array van PlayerShoot scripts
    public LevelSystem levelSystem; // Verwijzing naar het LevelSystem script
    public WeaponHolder weaponHolder; // Verwijzing naar de WeaponHolder script

    public static Shop Instance; // Singleton instance van de Shop

    void Awake()
    {
        // Controleert of er al een instance van Shop bestaat
        if (Instance == null)
        {
            Instance = this; // Stel deze instance in als de Singleton
            DontDestroyOnLoad(gameObject); // Voorkom dat dit object wordt vernietigd bij het laden van een scene
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Vernietig deze gameobject als er al een instance bestaat
        }
    }

    void Start()
    {
        // Initialiseer de array voor verkochte wapens
        isWeaponSold = new bool[gunriflePrice.Length];
        LoadState();
        LoadMedkitCount();
        UpdateUI();

        // Voeg een listener toe aan de koop extra leven knop
        buyLifeButton.onClick.AddListener(BuyLife);
    }

    void OnEnable()
    {
        // update de ui met de MoneyChanged gebeurtenis +
        GameControl.MoneyChanged += UpdateUI;
    }

    void OnDisable()
    {
        // update de ui met de MoneyChanged gebeurtenis -
        GameControl.MoneyChanged -= UpdateUI;
    }

    void LoadState()
    {
        // Laad de index van het equipped wapen
        equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", -1);
        // Laad de verkoopstatus van elk wapen
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = PlayerPrefs.GetInt("IsWeaponSold_" + i, 0) == 1;
        }
    }

    public void BuyMedkit()
    {
        int medkitPrice = int.Parse(medkitPriceText.text); // Prijs van de medkit
        // Controleer of de speler genoeg geld heeft
        if (GameControl.moneyAmount >= medkitPrice)
        {
            GameControl.Instance.ChangeMoney(-medkitPrice); // Verlaag het geldbedrag met de prijs van de medkit
            medkitCount++; // Verhoog het aantal medkits dat de player bezit
            PlayerPrefs.SetInt("MedkitCount", medkitCount); // Sla het nieuwe aantal op met playerprefs
            UpdateMedkitUI(); // Update de medkit UI
        }
    }

    void LoadMedkitCount()
    {
        // Laad het aantal medkits die opgeslagen waren
        medkitCount = PlayerPrefs.GetInt("MedkitCount", 0);
        UpdateMedkitUI(); // Update de medkit UI
    }

    public void BuyMagazine()
    {
        int magazinePrice = int.Parse(magazinePriceText.text); // Prijs van het magazijn
        // Controleer of de speler genoeg geld heeft en of het huidige wapen ammo kan krijgen
        if (GameControl.moneyAmount >= magazinePrice && playerShoots[equippedWeaponIndex].CanRefreshAmmo())
        {
            GameControl.Instance.ChangeMoney(-magazinePrice); // Verlaag het geldbedrag met de magazijn prijs
            playerShoots[equippedWeaponIndex].RefreshAmmo(); // Ververs de ammo van het wapen
            UpdateUI(); // Update de UI
        }
    }

    public void BuyLife()
    {
        int lifePrice = int.Parse(lifePriceText.text); // Prijs van een extra leven

        bool hasEnoughMoney = GameControl.moneyAmount >= lifePrice; // Controleer of de speler genoeg geld heeft
        bool canAddLife = playerHealth.GetLives() < playerHealth.maxLives; // Controleer of de speler minder dan het maximum aantal levens heeft

        // Als de speler genoeg geld heeft en een extra leven kan toevoegen
        if (hasEnoughMoney && canAddLife)
        {
            GameControl.Instance.ChangeMoney(-lifePrice); // Verlaag het geldbedrag met de prijs van een extra leven
            playerHealth.AddLife(); // Voeg een leven toe aan de speler
            UpdateUI(); // Update de UI
        }
    }

    void UpdateUI()
    {
        moneyAmountText.text = GameControl.moneyAmount.ToString(); // Update het geldbedrag in de UI
        int playerLevel = levelSystem.GetLevelAmount(); // Haal het huidige level aantal van de speler op

        // Update de interactie status van de koop magazijn knop
        if (equippedWeaponIndex >= 0 && equippedWeaponIndex < playerShoots.Length)
        {
            buyMagazineButton.interactable = playerShoots[equippedWeaponIndex].CanRefreshAmmo() && 
                GameControl.moneyAmount >= int.Parse(magazinePriceText.text);
        }
        else
        {
            buyMagazineButton.interactable = false;
        }

        // Update de interactie status van de koop medkit knop
        buyMedkitButton.interactable = GameControl.moneyAmount >= int.Parse(medkitPriceText.text) && playerLevel >= medkitLevelRequirement;

        // Update de medkit level requirement tekst
        if (playerLevel >= medkitLevelRequirement)
        {
            medkitLevelRequirementText.gameObject.SetActive(false);
        }
        else
        {
            medkitLevelRequirementText.gameObject.SetActive(true);
            medkitLevelRequirementText.text = "Level " + medkitLevelRequirement;
        }

        UpdateMedkitUI(); // Update de medkit UI

        // Update de UI voor elk wapen
        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            int weaponPrice = int.Parse(gunriflePrice[i].text);
            buyButton[i].gameObject.SetActive(!isWeaponSold[i]); // koop knop is active als wapen niet verkocht is
            buyButton[i].interactable = !isWeaponSold[i] && GameControl.moneyAmount >= weaponPrice && playerLevel >= levelRequirements[i]; // kan pas interacten met koop knop als wapen nog niet verkocht is, als speler genoeg geld heeft en als speler genoeg level heeft
            equipButton[i].gameObject.SetActive(isWeaponSold[i] && i != equippedWeaponIndex); // activate equip knop als de wapen verkocht is en als de wapen niet equipped is
            equippedText[i].gameObject.SetActive(i == equippedWeaponIndex); // activate de equipped tekst als de speler de wapen vast heeft

            if (playerLevel >= levelRequirements[i]) // als speler lever hoger of gelijk is aan required level om iets te kopen dan...
            {
                levelRequirementTexts[i].gameObject.SetActive(false); // zet de required level tekst inactive
            }
            else // als speler niet de juiste aantal level heeft dan...
            {
                levelRequirementTexts[i].gameObject.SetActive(true); // required level tekst is active
                levelRequirementTexts[i].text = "Level " + levelRequirements[i]; // het aantal level nodig om product te kunnen kopen
            }
        }

        int lifePrice = int.Parse(lifePriceText.text); // Prijs van een extra leven
        // Update de interactie status van de koop leven knop, dus je kan pas leven kopen als je genoeg geld hebt, als je minder levens hebt dan de max aantal en als je de nodige level hebt
        buyLifeButton.interactable = GameControl.moneyAmount >= lifePrice && playerHealth.GetLives() < playerHealth.maxLives && playerLevel >= lifeLevelRequirement;

        // Update de leven level required tekst
        if (playerLevel >= lifeLevelRequirement) // als de spelers level gelijk of hoger is dan de level nodig om een leven te kopen dan...
        {
            lifeLevelRequirementText.gameObject.SetActive(false); // zet de level requirement tekst inactive
        }
        else // als de spelers level lager is dan de nodige level dan...
        {
            lifeLevelRequirementText.gameObject.SetActive(true); // zet de level requirement tekst active
            lifeLevelRequirementText.text = "Level " + lifeLevelRequirement; // het aantal level nodig om een leven te kunnen kopen
        }
    }

    public void UpdateMedkitUI()
    {
        medkitCountText.text = medkitCount.ToString(); // Update het medkit aantal in de UI
    }

    public void BuyWeapon(int weaponIndex)
    {
        int weaponPrice = int.Parse(gunriflePrice[weaponIndex].text); // Prijs van het wapen
        // Controleer of het wapen niet verkocht is, de speler genoeg geld heeft en het nodige level heeft
        if (!isWeaponSold[weaponIndex] && GameControl.moneyAmount >= weaponPrice && levelSystem.GetLevelAmount() >= levelRequirements[weaponIndex])
        {
            GameControl.Instance.ChangeMoney(-weaponPrice); // Verlaag het geldbedrag met de wapenprijs
            isWeaponSold[weaponIndex] = true; // Markeer het wapen als verkocht
            PlayerPrefs.SetInt("IsWeaponSold_" + weaponIndex, 1); // Sla de verkoopstatus op in playerprefs
            EquipWeapon(weaponIndex); // equip de wapen meteen na dat je die koopt
            RefreshUI(); // Update de UI
        }
    }

    // reset alle playerprefs
    public void ResetMoneyAndWeapons()
    {
        GameControl.Instance.ChangeMoney(-GameControl.moneyAmount); // Reset het geldbedrag

        // Reset de verkoopstatus van alle wapens
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = false;
            PlayerPrefs.DeleteKey("IsWeaponSold_" + i);
        }

        equippedWeaponIndex = 8; // Stel een standaard wapen in wanneer je begint met het spelen
        isWeaponSold[equippedWeaponIndex] = true;
        PlayerPrefs.SetInt("IsWeaponSold_" + equippedWeaponIndex, 1);
        PlayerPrefs.SetInt("EquippedWeaponIndex", equippedWeaponIndex);

        medkitCount = 0; // Reset het aantal medkits
        PlayerPrefs.SetInt("MedkitCount", medkitCount);

        if (weaponHolder != null)
        {
            weaponHolder.ActivateWeapon(equippedWeaponIndex); // Activeer het standaard wapen
        }

        foreach (var playerShoot in playerShoots)
        {
            playerShoot.ResetAmmo(); // Reset de ammo van elk wapen
        }

        StatisticsManager.Instance.ResetStatistics(); // Reset de statistieken

        if (levelSystem != null)
        {
            levelSystem.ResetLevel(); // Reset het levelsysteem
        }

        PlayerPrefs.Save(); // Sla alle wijzigingen op zodat alles op gereset is
        RefreshUI(); // Update de UI
    }

    public void EquipWeapon(int weaponIndex)
    {
        equippedWeaponIndex = weaponIndex; // Stel het uitgeruste wapen in
        PlayerPrefs.SetInt("EquippedWeaponIndex", weaponIndex); // Sla de index van het uitgeruste wapen op
        RefreshUI(); // Update de UI
        gunChange.ActivateWeapon(weaponIndex); // Activeer het uitgeruste wapen
    }

    private void RefreshUI()
    {
        LoadState();
        UpdateUI();
    }
}