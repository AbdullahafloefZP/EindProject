using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    int weaponTotal = 1;
    [HideInInspector] public GameObject[] guns;
    public GameObject weaponHolder;

    void Awake() 
    {
        weaponTotal = weaponHolder.transform.childCount;
        guns = new GameObject[weaponTotal];

        for (int i = 0; i < weaponTotal; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }

        //guns[8].SetActive(true);
    }

    public void ActivateWeapon(int index)
    {
        if (index < 0 || index >= guns.Length)
        {
            return;
        }

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == index);
        }
    }
}
