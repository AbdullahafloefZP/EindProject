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
        timeSinceLastAttack += Time.deltaTime; // Verhoog de tijd sinds de laatste aanval met de verstreken tijd

        // Controleer of het tijd is om opnieuw aan te vallen
        if (timeSinceLastAttack >= attackInterval)
        {
            // Bereken de aangepaste positie voor de aanval
            Vector3 adjustedPosition = transform.position + new Vector3(0, 1.5f, 0);
            // Zoek naar spelers binnen het schadebereik
            Collider2D[] nearbyPlayers = Physics2D.OverlapCircleAll(adjustedPosition, damageRange);
            foreach (var playerCollider in nearbyPlayers)
            {
                if (playerCollider.CompareTag("Player"))
                {
                    // Speel het aanvals geluid af
                    audioManager.PlaySound(audioManager.zAttacking);
                    // Haal de PlayerHealth component van de speler op
                    PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        // damage de speler
                        playerHealth.TakeDamage(damageAmount);
                        // Reset de tijd sinds de laatste aanval
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