using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private int speed = 12;

    private Vector2 movement;
    private Rigidbody2D rb; 
    private Animator animator;
    public GameObject expCanvas;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        expCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            expCanvas.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            expCanvas.SetActive(false);
        }
    }

    private void OnMovement(InputValue value) 
    {
        movement = value.Get<Vector2>();

        if (movement.x != 0) 
        {
            animator.SetFloat("Y", 0);
            animator.SetBool("IsWalking", true);
        } else if (movement.y != 0) {
            animator.SetFloat("Y", movement.y);
            animator.SetBool("IsWalking", true);
        } 
        else 
        {
            animator.SetBool("IsWalking", false);
        }

    }

    private void OnMouseMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;

        if (direction.x > 0) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } 
        else if (direction.x < 0) 
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void ResetPosition()
    {
       transform.position = new Vector2(0, 0);
       rb.velocity = Vector2.zero;
       rb.angularVelocity = 0;
       rb.isKinematic = false;
    }

    private void FixedUpdate() 
    {  
        if (transform.position.y >= 68)
        {
            transform.position = new Vector3(transform.position.x, 68, 0);
        }
        else if (transform.position.y <= -95)
        {
            transform.position = new Vector3(transform.position.x, -95, 0);
        }
        if (transform.position.x >= 110)
        {
            transform.position = new Vector3(110, transform.position.y, 0);
        }
        else if (transform.position.x <= -160)
        {
            transform.position = new Vector3(-160, transform.position.y, 0);
        }

        OnMouseMovement();
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        rb.freezeRotation = true;
    }
    
}