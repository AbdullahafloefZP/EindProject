using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private int speed = 12;

    private Vector2 movement;
    private Rigidbody2D rb; 
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnMovement(InputValue value) {
        movement = value.Get<Vector2>();

        if (movement.x != 0) {
            //animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", 0);
            animator.SetBool("IsWalking", true);
         } else if (movement.y != 0) {
            //animator.SetFloat("X", 0);
            animator.SetFloat("Y", movement.y);
            animator.SetBool("IsWalking", true);
        } else {
            animator.SetBool("IsWalking", false);
    }

    }

    private void OnMouseMovement() {
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 direction = mousePosition - transform.position;
        
   if (direction.x > 0) {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    } else if (direction.x < 0) {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}

    private void FixedUpdate() {    
        OnMouseMovement();
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        rb.freezeRotation = true;
    }
    
}