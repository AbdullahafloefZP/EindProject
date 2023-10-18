using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform weaponsParent;
    private Transform shootPoint;

    private void Awake()
    {
        weaponsParent = transform.Find("Weapons");
        aimTransform = weaponsParent.Find("Ak47");
        shootPoint = aimTransform.Find("Shootpoint");
    }

    private void Update()
    {
        UpdateGunRotation();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        weaponsParent.eulerAngles = new Vector3(0, 0, angle);

        shootPoint.position = mousePosition;
    }

    private void UpdateGunRotation()
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