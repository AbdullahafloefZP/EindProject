using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGiveDamage : MonoBehaviour
{
    [SerializeField] private float damageRange = 1.5f;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackInterval = 3f;

    private float timeSinceLastAttack;

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= attackInterval)
        {
            Collider2D[] nearbyPlayers = Physics2D.OverlapCircleAll(transform.position, damageRange);
            foreach (var playerCollider in nearbyPlayers)
            {
                if (playerCollider.CompareTag("Player"))
                {
                    PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageAmount);
                        timeSinceLastAttack = 0f;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
