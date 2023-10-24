using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    public LineRenderer lineRenderer;
    public float shootRate = 0.2f;
    private bool isShooting = false;
    private float lastShootTime = 2f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isShooting = true;
            Shoot();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isShooting = false;
            lineRenderer.enabled = false;
        }

        if (isShooting && Time.time - lastShootTime >= shootRate)
        {
            isShooting = true;
            Shoot();
        } 

        if (isShooting && Time.time - lastShootTime >= 0.1f) {
            lineRenderer.enabled = false;
        }
    }

    private void Shoot()
    {
        lastShootTime = Time.time;
        lineRenderer.enabled = true;

        RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, gunTransform.right);

        if (hit)
        {
            lineRenderer.SetPosition(0, gunTransform.position);
            lineRenderer.SetPosition(1, hit.point);

            DamageFlash damageFlash = hit.collider.GetComponent<DamageFlash>();
            if (damageFlash != null)
            {
                damageFlash.CallDamageFlash();
                hit.collider.gameObject.TryGetComponent<DamageFlash>(out DamageFlash zombieComponent);
                if (zombieComponent != null)
                {
                    zombieComponent.TakeDamage(1);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, gunTransform.position);
            lineRenderer.SetPosition(1, gunTransform.position + gunTransform.right * 100);
        }
    }
}
