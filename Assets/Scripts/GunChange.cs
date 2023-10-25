using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChange : MonoBehaviour
{
    public WeaponHolder weapon;

    void Awake() 
    {
        weapon = GetComponentInChildren<WeaponHolder>();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        GameObject whatHit = collision.gameObject;

        if (whatHit.CompareTag("Ak47"))
        {
            weapon.guns[0].SetActive(true);
            weapon.guns[1].SetActive(false);
            weapon.guns[2].SetActive(false);
            weapon.guns[3].SetActive(false);
            
        }

        if (whatHit.CompareTag("ScarL"))
        {
            weapon.guns[0].SetActive(false);
            weapon.guns[1].SetActive(true);
            weapon.guns[2].SetActive(false);
            weapon.guns[3].SetActive(false);
            
        }

        if (whatHit.CompareTag("FRD"))
        {
            weapon.guns[0].SetActive(false);
            weapon.guns[1].SetActive(false);
            weapon.guns[2].SetActive(true);
            weapon.guns[3].SetActive(false);
            
        }

        if (whatHit.CompareTag("N4o1"))
        {
            weapon.guns[0].SetActive(false);
            weapon.guns[1].SetActive(false);
            weapon.guns[2].SetActive(false);
            weapon.guns[3].SetActive(true);
            
        }   
    }
}
