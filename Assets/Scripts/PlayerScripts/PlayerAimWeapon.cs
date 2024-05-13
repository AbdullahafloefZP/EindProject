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

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        UpdateGunRotation();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                Vector3 aimDirection = (mousePosition - child.position).normalized;

                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                child.eulerAngles = new Vector3(0, 0, angle);

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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;

        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                SpriteRenderer gunSpriteRenderer = child.GetComponent<SpriteRenderer>();

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
