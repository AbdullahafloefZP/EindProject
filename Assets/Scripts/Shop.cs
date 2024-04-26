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

    private int equippedWeaponIndex = -1;
    private bool[] isWeaponSold;

    void OnEnable()
    {
        GameControl.MoneyChanged += UpdateUI;
    }

    void OnDisable()
    {
        GameControl.MoneyChanged -= UpdateUI;
    }

    void Start()
    {
        isWeaponSold = new bool[gunriflePrice.Length];
        LoadState();
        UpdateUI();
    }

    void LoadState()
    {
        equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", -1);
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = PlayerPrefs.GetInt("IsWeaponSold_" + i, 0) == 1;
        }
    }

    void UpdateUI()
    {
        moneyAmountText.text = GameControl.moneyAmount.ToString();
        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            int weaponPrice = int.Parse(gunriflePrice[i].text);
            buyButton[i].gameObject.SetActive(!isWeaponSold[i]);
            buyButton[i].interactable = !isWeaponSold[i] && GameControl.moneyAmount >= weaponPrice;
            equipButton[i].gameObject.SetActive(isWeaponSold[i] && i != equippedWeaponIndex);
            equippedText[i].gameObject.SetActive(i == equippedWeaponIndex);
        }
    }

    public void BuyWeapon(int weaponIndex)
    {
        int weaponPrice = int.Parse(gunriflePrice[weaponIndex].text);
        if (!isWeaponSold[weaponIndex] && GameControl.moneyAmount >= weaponPrice)
        {
            GameControl.Instance.ChangeMoney(-weaponPrice); // Deduct the weapon price from money
            isWeaponSold[weaponIndex] = true;
            PlayerPrefs.SetInt("IsWeaponSold_" + weaponIndex, 1);
            EquipWeapon(weaponIndex); // Automatically equip the weapon on purchase
            RefreshUI();
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        equippedWeaponIndex = weaponIndex;
        PlayerPrefs.SetInt("EquippedWeaponIndex", weaponIndex);
        RefreshUI();
        gunChange.ActivateWeapon(weaponIndex);
    }

    public void ResetMoneyAndWeapons()
    {
        GameControl.Instance.ChangeMoney(-GameControl.moneyAmount); // Set money to 0
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = false;
            PlayerPrefs.DeleteKey("IsWeaponSold_" + i);
        }
        equippedWeaponIndex = -1;
        PlayerPrefs.SetInt("EquippedWeaponIndex", -1);
        PlayerPrefs.Save(); // Ensure all changes are saved to PlayerPrefs immediately
        RefreshUI();
    }

    private void RefreshUI()
    {
        LoadState(); // Reloads the saved state from PlayerPrefs
        UpdateUI(); // Updates the UI based on the loaded state
    }
}
