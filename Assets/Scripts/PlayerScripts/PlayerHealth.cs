using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;
    public HealthBar healthBar;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseMedkit();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void ResetHealth() 
    {
        health = maxHealth;
        healthBar.SetHealth(maxHealth);
        gameObject.SetActive(true);
    }

    public void UseMedkit()
    {
        if (Shop.Instance.medkitCount > 0)
        {
            health = maxHealth;
            healthBar.SetHealth(health);
            Shop.Instance.medkitCount--;

            PlayerPrefs.SetInt("MedkitCount", Shop.Instance.medkitCount);
            PlayerPrefs.Save();

            Shop.Instance.UpdateMedkitUI();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        LoseMenu.PlayerHasDied = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(2);
        }
    }
}
