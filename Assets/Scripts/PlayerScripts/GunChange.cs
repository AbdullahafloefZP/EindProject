using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChange : MonoBehaviour
{
    public WeaponHolder weapon;

    void Start()
    {
        int equippedWeaponIndex = PlayerPrefs.GetInt("EquippedWeaponIndex", 0);
        ActivateWeapon(equippedWeaponIndex);
    }

    public void ActivateWeapon(int index)
    {
        if (index >= 0 && index < weapon.guns.Length)
        {
            foreach (GameObject gun in weapon.guns)
            {
                gun.SetActive(gun == weapon.guns[index]);
            }
        }
    }
}
