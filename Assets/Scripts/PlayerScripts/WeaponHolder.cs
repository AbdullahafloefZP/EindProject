using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    int weaponTotal = 1;
    // public static int currentWeaponIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    // public GameObject currentWeapon;

    void Awake() 
    {
        weaponTotal = weaponHolder.transform.childCount;
        guns =  new GameObject[weaponTotal];

        for (int i = 0; i < weaponTotal; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }

        guns[0].SetActive(true);
        // currentWeapon = guns[0];
        // currentWeaponIndex = 0;
    }
}
