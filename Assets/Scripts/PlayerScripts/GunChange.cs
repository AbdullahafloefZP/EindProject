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
        foreach (GameObject gun in weapon.guns)
        {
            if (gun.tag == weaponTag)
            {
                gun.SetActive(true);
            }
            else
            {
                gun.SetActive(false);
            }
        }

        pickupMessage.SetActive(false);
        Destroy(currentWeapon);
    }

    bool IsWeaponTag(string tag)
    {
        return tag == "Ak47" || tag == "ScarL" || tag == "FRD" || tag == "N4o1" || tag == "Mg401";
    }
}
