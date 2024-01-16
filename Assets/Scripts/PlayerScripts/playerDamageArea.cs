using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDamageArea : MonoBehaviour
{
    public int damageAmount = 2;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamageToPlayer(other.gameObject);
        }
    }

    private void DealDamageToPlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
