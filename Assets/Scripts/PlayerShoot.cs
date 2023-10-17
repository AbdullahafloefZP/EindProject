using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private LayerMask shootableLayers;
    [SerializeField] private Transform gunTransform;
    
    private float shootCooldownTimer;
    private Vector2 shootDirection;
    
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && shootCooldownTimer <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        
        Vector3 shootOrigin = gunTransform.position;
        shootDirection = gunTransform.right; 

        
        RaycastHit2D hit = Physics2D.Raycast(shootOrigin, shootDirection, shootableLayers);

        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;
        }
        Debug.Log(hit.transform.name);
    }
}
