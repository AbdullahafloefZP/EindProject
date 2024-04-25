using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public Text moneyText;
    public static int moneyAmount;
    public WeaponHolder weaponHolder;

    void Start()
    {
        moneyAmount = PlayerPrefs.GetInt("MoneyAmount");
        UpdateMoneyDisplay();
        int equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", -1);
        ActivatePurchasedWeapon(equippedWeaponIndex);
    }

    void Update()
    {
        UpdateMoneyDisplay();
    }

    void UpdateMoneyDisplay()
    {
        moneyText.text = moneyAmount.ToString();
    }

    void ActivatePurchasedWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponHolder.guns.Length)
        {
            foreach (GameObject gun in weaponHolder.guns)
            {
                gun.SetActive(false);
            }
            weaponHolder.guns[weaponIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid weapon index: " + weaponIndex);
        }
    }
}

