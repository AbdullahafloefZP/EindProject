using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChange : MonoBehaviour
{
    // Referentie naar de WeaponHolder script
    public WeaponHolder weapon;

    void Start()
    {
        // Haal de index van het uitgeruste wapen op uit de PlayerPrefs
        int equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", 0);
        // Activeer het wapen met de opgehaalde index
        ActivateWeapon(equippedWeaponIndex);
    }

    // Functie om een wapen te activeren op basis van de gegeven index
    public void ActivateWeapon(int index)
    {
        // Controleer of de index binnen het bereik van de wapens array ligt
        if (index >= 0 && index < weapon.guns.Length)
        {
            // Loop door alle wapens in de WeaponHolder
            foreach (GameObject gun in weapon.guns)
            {
                // Activeer het wapen als het de opgegeven index heeft
                gun.SetActive(gun == weapon.guns[index]);
            }
        }
    }
}