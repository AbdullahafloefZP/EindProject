using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform weaponsParent;
    private Transform Canvas;
    private Transform Canvas2;

    private void Awake()
    {
        weaponsParent = transform.Find("Weapons");
        Canvas = transform.Find("Canvas/reloadMessage");
        Canvas2 = transform.Find("Canvas/ammoMessage");
    }

    // Update wordt elke frame aangeroepen
    private void Update()
    {
        // stop alles als het spel is gepauzeerd
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        UpdateGunRotation();

        // Haal de muispositie op in wereldcoördinaten
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Loop door alle objecten die in weaponsParent zitten en update hun rotatie en canvas oriëntatie
        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                // Bereken de richtingsvector naar de muis
                Vector3 aimDirection = (mousePosition - child.position).normalized;

                // Bereken de hoek in graden
                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                child.eulerAngles = new Vector3(0, 0, angle); // Zet de rotatie van het wapen

                // Update de oriëntatie van de berichten op basis van de richting van de speler
                if (transform.localScale.x < 0)
                {
                    Canvas.localEulerAngles = new Vector3(0, 180, 0);
                    Canvas2.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    Canvas.localEulerAngles = Vector3.zero;
                    Canvas2.localEulerAngles = Vector3.zero;
                }
            }
        }
    }

    private void UpdateGunRotation()
    {
        // Haal de muispositie op in wereldcoördinaten
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        // Loop door alle objecten die in weaponsParent zitten en update hun schaal op basis van de richting naar de muis
        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                SpriteRenderer gunSpriteRenderer = child.GetComponent<SpriteRenderer>();

                // Update de schaal van het wapen op basis van de richting naar de muis
                if (direction.x > 0)
                {
                    child.localScale = new Vector3(Mathf.Abs(child.localScale.x), child.localScale.y, child.localScale.z);
                    child.localScale = new Vector3(child.localScale.x, Mathf.Abs(child.localScale.y), child.localScale.z);
                }
                else if (direction.x < 0)
                {
                    child.localScale = new Vector3(-Mathf.Abs(child.localScale.x), child.localScale.y, child.localScale.z);
                    child.localScale = new Vector3(child.localScale.x, -Mathf.Abs(child.localScale.y), child.localScale.z);
                }
            }
        }
    }
}