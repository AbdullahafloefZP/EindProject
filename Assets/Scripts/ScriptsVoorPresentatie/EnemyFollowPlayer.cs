using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSight;

    private Transform player;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Controleer of de winkel actief is en stop de enemies
        if (ShopTrigger.IsShopActive)
        {
            movement = Vector2.zero;
            SetAnimationParameters(movement);
            return;
        }

        // Bereken de afstand van de vijand tot de speler
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        Vector2 moveDirection;

        // Als de speler binnen het zichtbereik is, beweeg naar de speler toe
        if (distanceFromPlayer < lineOfSight)
        {
            moveDirection = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector2.zero;
        }

        // Stel de animatieparameters in op basis van de bewegingsrichting
        SetAnimationParameters(moveDirection);
    }

    // Stel de animatieparameters in
    private void SetAnimationParameters(Vector2 moveDirection)
    {
        animator.SetFloat("X", moveDirection.x);
        animator.SetFloat("Y", moveDirection.y);
        animator.SetBool("IsWalking", moveDirection != Vector2.zero);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        rb.freezeRotation = true;
    }
}