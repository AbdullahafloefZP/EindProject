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

        int purchasedWeaponIndex = PlayerPrefs.GetInt("PurchasedWeaponIndex", -1);
        if (purchasedWeaponIndex != -1)
        {
            ActivatePurchasedWeapon(purchasedWeaponIndex);
            if (weaponHolder.guns[purchasedWeaponIndex].activeSelf)
            {
                //PlayerPrefs.DeleteKey("PurchasedWeaponIndex");
            }
            else
            {
                Debug.LogWarning("Failed to activate purchased weapon: " + purchasedWeaponIndex);
            }
        }

    }

    void Update()
    {
        moneyText.text = moneyAmount.ToString();
    }

    public void GotoShop()
    {
        PlayerPrefs.SetInt("MoneyAmount", moneyAmount);
        SceneManager.LoadScene("ShopScene");
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
