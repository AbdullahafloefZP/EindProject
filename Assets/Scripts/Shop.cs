using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    int moneyAmount;
    bool[] isWeaponSold;

    public Text moneyAmountText;
    public Text[] gunriflePrice;
    public Button[] buyButton;
    public Button[] equipButton;
    public Text[] equippedText;
    public GunChange gunChange;

    void Start()
    {
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        moneyAmountText.text = moneyAmount.ToString(); // Update UI with initial money amount
        isWeaponSold = new bool[gunriflePrice.Length];
        int equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", -1);

        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            isWeaponSold[i] = PlayerPrefs.GetInt("IsWeaponSold_" + i, 0) == 1;
            buyButton[i].gameObject.SetActive(!isWeaponSold[i]);
            equipButton[i].gameObject.SetActive(isWeaponSold[i] && i != equippedWeaponIndex);
            equippedText[i].gameObject.SetActive(i == equippedWeaponIndex);
        }
        UpdateButtonInteractability();
    }

    void UpdateButtonInteractability()
{
    for (int i = 0; i < gunriflePrice.Length; i++)
    {
        bool canBuy = !isWeaponSold[i] && moneyAmount >= int.Parse(gunriflePrice[i].text);
        buyButton[i].interactable = canBuy;
        Debug.Log($"UpdateButtonInteractability: Index {i}, canBuy {canBuy}, isWeaponSold {isWeaponSold[i]}, moneyAmount {moneyAmount}, WeaponPrice {gunriflePrice[i].text}");
    }
}


    public void BuyWeapon(int weaponIndex)
    {
        int weaponPrice = int.Parse(gunriflePrice[weaponIndex].text); // Get the price of the weapon
        if (!isWeaponSold[weaponIndex] && moneyAmount >= weaponPrice)
        {
            moneyAmount -= weaponPrice; // Subtract the weapon price from the money amount
            moneyAmountText.text = moneyAmount.ToString(); // Update UI with the new money amount
            isWeaponSold[weaponIndex] = true;
            PlayerPrefs.SetInt("IsWeaponSold_" + weaponIndex, 1);
            PlayerPrefs.SetInt("MoneyAmount", moneyAmount);
            RefreshUI(weaponIndex);
            gunChange.ActivateWeapon(weaponIndex);
        }
    }

    public void EquipWeapon(int weaponIndex)
    {
        PlayerPrefs.SetInt("EquippedWeaponIndex", weaponIndex);
        RefreshUI(weaponIndex);
        gunChange.ActivateWeapon(weaponIndex);
    }

    public void ResetMoneyAndWeapons()
    {
        PlayerPrefs.DeleteKey("MoneyAmount");  // Deletes the saved money amount
        moneyAmount = 0;  // Resets the money amount in the current session

        // Reset each weapon's sold state and PlayerPrefs
        for (int i = 0; i < isWeaponSold.Length; i++)
        {
            isWeaponSold[i] = false;
            PlayerPrefs.DeleteKey("IsWeaponSold_" + i);
        }

        PlayerPrefs.SetInt("EquippedWeaponIndex", -1);  // Optionally reset equipped weapon index if needed
        UpdateUI();
    }

    private void UpdateUI()
    {
        moneyAmountText.text = moneyAmount.ToString();
        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            buyButton[i].gameObject.SetActive(true);
            equipButton[i].gameObject.SetActive(false);
            equippedText[i].gameObject.SetActive(false);
        }
        UpdateButtonInteractability();
    }

    private void RefreshUI(int activeWeaponIndex)
    {
        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            buyButton[i].gameObject.SetActive(!isWeaponSold[i]);
            equipButton[i].gameObject.SetActive(isWeaponSold[i] && i != activeWeaponIndex);
            equippedText[i].gameObject.SetActive(i == activeWeaponIndex);
        }
    }
}