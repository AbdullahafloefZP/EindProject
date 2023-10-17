using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform weaponsParent;

    private void Awake()
    {
        weaponsParent = transform.Find("Weapons");
        aimTransform = weaponsParent.Find("Ak47");
    }

    private void Update()
    {
        UpdateGunFlip();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void UpdateGunFlip()
    {
        if (aimTransform != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePosition - transform.position;
            SpriteRenderer gunSpriteRenderer = aimTransform.GetComponent<SpriteRenderer>();

            if (direction.x > 0)
            {
                gunSpriteRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
                aimTransform.localScale = new Vector3(Mathf.Abs(aimTransform.localScale.x), aimTransform.localScale.y, aimTransform.localScale.z);
                aimTransform.localScale = new Vector3(aimTransform.localScale.x, Mathf.Abs(aimTransform.localScale.y), aimTransform.localScale.z);
            }
            else if (direction.x < 0)
            {
                gunSpriteRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
                aimTransform.localScale = new Vector3(-Mathf.Abs(aimTransform.localScale.x), aimTransform.localScale.y, aimTransform.localScale.z);
                aimTransform.localScale = new Vector3(aimTransform.localScale.x, -Mathf.Abs(aimTransform.localScale.y), aimTransform.localScale.z);
            }
        }
    }
}
