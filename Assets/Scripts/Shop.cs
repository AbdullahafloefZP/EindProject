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
    public Text[] soldText;
    public Button[] buyButton;

    void Start()
    {
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        isWeaponSold = new bool[gunriflePrice.Length];
        
        foreach (Text soldTextElement in soldText)
        {
            soldTextElement.gameObject.SetActive(false);
        }
        UpdateButtonInteractability();
    }

    void Update()
    {
        moneyAmountText.text = moneyAmount.ToString();
    }

    void UpdateButtonInteractability()
    {
        for (int i = 0; i < gunriflePrice.Length; i++)
        {
            if (isWeaponSold[i])
            {
                buyButton[i].interactable = false;
            }
            else
            {
                buyButton[i].interactable = moneyAmount >= int.Parse(gunriflePrice[i].text);
            }
        }
    }

    public void BuyWeapon(int weaponIndex)
    {
        if (!isWeaponSold[weaponIndex] && moneyAmount >= int.Parse(gunriflePrice[weaponIndex].text))
        {
            moneyAmount -= int.Parse(gunriflePrice[weaponIndex].text);
            isWeaponSold[weaponIndex] = true;
            soldText[weaponIndex].gameObject.SetActive(true);
            buyButton[weaponIndex].gameObject.SetActive(false);
            UpdateButtonInteractability();

            PlayerPrefs.SetInt("PurchasedWeaponIndex", weaponIndex);
        }
    }

    public void ExitShop()
    {
        PlayerPrefs.SetInt("MoneyAmount", moneyAmount);
        SceneManager.LoadScene("MainGame");
    }
}
