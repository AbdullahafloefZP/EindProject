using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text moneyAmountText;
    public Text[] gunriflePrice;
    public Button[] buyButton;
    public Button[] equipButton;
    public Text[] equippedText;
    public GunChange gunChange;
    public Text[] levelRequirementTexts;
    public int[] levelRequirements;
    public int medkitLevelRequirement;
    public Text medkitLevelRequirementText;
    public Text medkitPriceText;
    public Button buyMedkitButton;
    public Text magazinePriceText;
    public Button buyMagazineButton;
    public Text medkitCountText;
    private int equippedWeaponIndex = -1;
    private bool[] isWeaponSold;
    [HideInInspector] public int medkitCount = 0;
    public PlayerShoot[] playerShoots;
    public LevelSystem levelSystem;

    public static Shop Instance;

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
        isWeaponSold = new bool[gunriflePrice.Length];
        LoadState();
        LoadMedkitCount();
        UpdateUI();
    }

    void OnEnable()
    {
        GameControl.MoneyChanged += UpdateUI;
    }

    void OnDisable()
    {
        GameControl.MoneyChanged -= UpdateUI;
    }

     void LoadState()
    {
        equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", -1);
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = PlayerPrefs.GetInt("IsWeaponSold_" + i, 0) == 1;
        }
    }

     public void BuyMedkit()
    {
        int medkitPrice = int.Parse(medkitPriceText.text);
        if (GameControl.moneyAmount >= medkitPrice)
        {
            GameControl.Instance.ChangeMoney(-medkitPrice);
            medkitCount++;
            PlayerPrefs.SetInt("MedkitCount", medkitCount);
            UpdateMedkitUI();
        }
    }

    void LoadMedkitCount()
    {
        medkitCount = PlayerPrefs.GetInt("MedkitCount", 0);
        UpdateMedkitUI();
    }

    public void BuyMagazine()
    {
        int magazinePrice = int.Parse(magazinePriceText.text);
        if (GameControl.moneyAmount >= magazinePrice && playerShoots[equippedWeaponIndex].CanRefreshAmmo())
        {
            GameControl.Instance.ChangeMoney(-magazinePrice);
            playerShoots[equippedWeaponIndex].RefreshAmmo();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        moneyAmountText.text = GameControl.moneyAmount.ToString();
        int playerLevel = levelSystem.GetLevelAmount();

        if (equippedWeaponIndex >= 0 && equippedWeaponIndex < playerShoots.Length)
        {
            buyMagazineButton.interactable = playerShoots[equippedWeaponIndex].CanRefreshAmmo() && 
                GameControl.moneyAmount >= int.Parse(magazinePriceText.text);
        }
        else
        {
            buyMagazineButton.interactable = false;
        }

        buyMedkitButton.interactable = GameControl.moneyAmount >= int.Parse(medkitPriceText.text) && playerLevel >= medkitLevelRequirement;

        if (playerLevel >= medkitLevelRequirement)
        {
            medkitLevelRequirementText.gameObject.SetActive(false);
        }
        else
        {
            medkitLevelRequirementText.gameObject.SetActive(true);
            medkitLevelRequirementText.text = "Level " + medkitLevelRequirement;
        }

        UpdateMedkitUI();

        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            int weaponPrice = int.Parse(gunriflePrice[i].text);
            buyButton[i].gameObject.SetActive(!isWeaponSold[i]);
            buyButton[i].interactable = !isWeaponSold[i] && GameControl.moneyAmount >= weaponPrice && playerLevel >= levelRequirements[i];
            equipButton[i].gameObject.SetActive(isWeaponSold[i] && i != equippedWeaponIndex);
            equippedText[i].gameObject.SetActive(i == equippedWeaponIndex);

            if (playerLevel >= levelRequirements[i])
            {
                levelRequirementTexts[i].gameObject.SetActive(false);
            }
            else
            {
                levelRequirementTexts[i].gameObject.SetActive(true);
                levelRequirementTexts[i].text = "Level " + levelRequirements[i];
            }
        }
    }


    public void UpdateMedkitUI()
    {
        medkitCountText.text = medkitCount.ToString();
    }

    public void BuyWeapon(int weaponIndex)
    {
        int weaponPrice = int.Parse(gunriflePrice[weaponIndex].text);
        if (!isWeaponSold[weaponIndex] && GameControl.moneyAmount >= weaponPrice && levelSystem.GetLevelAmount() >= levelRequirements[weaponIndex])
        {
            GameControl.Instance.ChangeMoney(-weaponPrice);
            isWeaponSold[weaponIndex] = true;
            PlayerPrefs.SetInt("IsWeaponSold_" + weaponIndex, 1);
            EquipWeapon(weaponIndex);
            RefreshUI();
        }
    }

    public void ResetMoneyAndWeapons()
    {
        GameControl.Instance.ChangeMoney(-GameControl.moneyAmount);

        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = false;
            PlayerPrefs.DeleteKey("IsWeaponSold_" + i);
        }

        equippedWeaponIndex = -1;
        PlayerPrefs.SetInt("EquippedWeaponIndex", -1);

        medkitCount = 0;
        PlayerPrefs.SetInt("MedkitCount", medkitCount);

        if (levelSystem != null)
        {
            levelSystem.ResetLevel();
        }
        else
        {
            Debug.LogError("LevelSystem not set in Shop.");
        }

        PlayerPrefs.Save();
        RefreshUI();
    }


    public void EquipWeapon(int weaponIndex)
    {
        equippedWeaponIndex = weaponIndex;
        PlayerPrefs.SetInt("EquippedWeaponIndex", weaponIndex);
        RefreshUI();
        gunChange.ActivateWeapon(weaponIndex);
    }

    private void RefreshUI()
    {
        LoadState();
        UpdateUI();
    }
}
