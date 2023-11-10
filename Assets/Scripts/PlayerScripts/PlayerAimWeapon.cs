using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform weaponsParent;
    private Transform weaponsCanvas;

    private void Awake()
    {
        weaponsParent = transform.Find("Weapons");
        weaponsCanvas = transform.Find("Canvas/pickupMessage");
    }

    private void Update()
    {
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
                    weaponsCanvas.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    weaponsCanvas.localEulerAngles = Vector3.zero;
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