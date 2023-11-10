using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChange : MonoBehaviour
{
    public WeaponHolder weapon;
    public GameObject pickupMessage;
    private bool canPickup;
    private GameObject currentWeapon;

    void Awake()
    {
        weapon = GetComponentInChildren<WeaponHolder>();
    }

    void Start()
    {
        canPickup = false;
        pickupMessage.SetActive(false);
    }

    void Update()
    {
        if (canPickup)
        {
            if (Input.GetKeyDown(KeyCode.E) && currentWeapon != null)
            {
                string weaponTag = currentWeapon.tag;
                if (!string.IsNullOrEmpty(weaponTag))
                {
                    PickUpWeapon(weaponTag);
                }
            }
        }
    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsWeaponTag(other.tag))
        {
            canPickup = true;
            pickupMessage.SetActive(true);
            currentWeapon = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsWeaponTag(other.tag))
        {
            canPickup = false;
            pickupMessage.SetActive(false);
            currentWeapon = null;
        }
    }

    void PickUpWeapon(string weaponTag)
    {
        switch (weaponTag)
        {
            case "Ak47":
                weapon.guns[0].SetActive(true);
                weapon.guns[1].SetActive(false);
                weapon.guns[2].SetActive(false);
                weapon.guns[3].SetActive(false);
                break;

            case "ScarL":
                weapon.guns[0].SetActive(false);
                weapon.guns[1].SetActive(true);
                weapon.guns[2].SetActive(false);
                weapon.guns[3].SetActive(false);
                break;

            case "FRD":
                weapon.guns[0].SetActive(false);
                weapon.guns[1].SetActive(false);
                weapon.guns[2].SetActive(true);
                weapon.guns[3].SetActive(false);
                break;

            case "N4o1":
                weapon.guns[0].SetActive(false);
                weapon.guns[1].SetActive(false);
                weapon.guns[2].SetActive(false);
                weapon.guns[3].SetActive(true);
                break;
        }
        pickupMessage.SetActive(false);
        Destroy(currentWeapon);
        
    }

    bool IsWeaponTag(string tag)
    {
        return tag == "Ak47" || tag == "ScarL" || tag == "FRD" || tag == "N4o1";
    }
}
