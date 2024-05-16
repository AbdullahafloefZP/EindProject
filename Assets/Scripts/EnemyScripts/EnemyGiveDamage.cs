using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGiveDamage : MonoBehaviour
{
    [SerializeField] private float damageRange = 1.5f;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float attackInterval = 3f;

    private float timeSinceLastAttack;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack >= attackInterval)
        {
            Vector3 adjustedPosition = transform.position + new Vector3(0, 1.5f, 0);
            Collider2D[] nearbyPlayers = Physics2D.OverlapCircleAll(adjustedPosition, damageRange);
            foreach (var playerCollider in nearbyPlayers)
            {
                if (playerCollider.CompareTag("Player"))
                {
                    audioManager.PlaySound(audioManager.zAttacking);
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
        Vector3 adjustedPosition = transform.position + new Vector3(0, 1.5f, 0);
        Gizmos.DrawWireSphere(adjustedPosition, damageRange);
    }
}
