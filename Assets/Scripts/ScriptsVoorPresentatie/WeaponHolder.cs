using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // Het totale aantal wapens, standaard ingesteld op 1
    int weaponTotal = 1;
    // Array om de wapens in op te slaan
    [HideInInspector] public GameObject[] guns;
    // Verwijzing naar de weaponholder script
    public GameObject weaponHolder;

    // Deze functie wordt aangeroepen wanneer het object wordt geïnitialiseerd
    void Awake() 
    {
        // Stel het totale aantal wapens in op basis van het aantal objecten die in weaponHolder zitten
        weaponTotal = weaponHolder.transform.childCount;
        // Initialiseer de guns array met de juiste grootte
        guns = new GameObject[weaponTotal];

        // Loop door elk object die in weaponHolder zit en voeg het toe aan de guns array
        for (int i = 0; i < weaponTotal; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false); // Zet elk wapen inactief bij het opstarten
        }
    }

    // Functie om een specifiek wapen te activeren
    public void ActivateWeapon(int index)
    {
        // Controleer of de index binnen het bereik van de array ligt
        if (index < 0 || index >= guns.Length)
        {
            return; // Stop de functie als de index ongeldig is
        }

        // Loop door elk wapen en activeer alleen het wapen met de gegeven index
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(i == index); // Zet het wapen actief als de index overeenkomt
        }
    }
}