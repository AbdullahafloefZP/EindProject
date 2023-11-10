using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lineOfSight;
    [SerializeField] private float avoidanceDistance = 3f;

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
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        Vector2 moveDirection;

        if (distanceFromPlayer < lineOfSight)
        {
            moveDirection = (player.position - transform.position).normalized;

            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceDistance);
            foreach (var enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.CompareTag("Enemy") && enemyCollider.transform != transform)
                {
                    Vector2 avoidanceDirection = (transform.position - enemyCollider.transform.position).normalized;
                    float dotProduct = Vector2.Dot(moveDirection, avoidanceDirection);

                    if (dotProduct > 0.5f)
                    {
                        moveDirection += avoidanceDirection;
                    }
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector2.zero;
        }

        SetAnimationParameters(moveDirection);
    }

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
