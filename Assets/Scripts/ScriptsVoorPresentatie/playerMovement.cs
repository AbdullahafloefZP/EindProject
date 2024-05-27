using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // De snelheid van de speler
    [SerializeField] private int speed = 12;

    // Variabelen voor beweging en componenten
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject expCanvas;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Haal de Rigidbody2D component op
        animator = GetComponent<Animator>(); // Haal de Animator component op
        expCanvas.SetActive(false);
    }

    // Update wordt elke frame aangeroepen
    void Update()
    {
        // Toon het ervaringscanvas wanneer de Tab-toets wordt ingedrukt
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            expCanvas.SetActive(true);
        }
        // Verberg het ervaringscanvas wanneer de Tab-toets wordt losgelaten
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            expCanvas.SetActive(false);
        }
    }

    // Deze methode wordt aangeroepen wanneer er beweging is
    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>(); // Haal de beweging vector op

        // Update de animator parameters op basis van de bewegingsvector
        if (movement.x != 0)
        {
            animator.SetFloat("Y", 0);
            animator.SetBool("IsWalking", true);
        }
        else if (movement.y != 0)
        {
            animator.SetFloat("Y", movement.y);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    // Methode om de speler richting de muis te draaien
    private void OnMouseMovement()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Haal de muispositie op in wereldcoördinaten
        Vector2 direction = mousePosition - transform.position; // Bereken de richting naar de muis

        // Draai de speler naar de juiste kant op basis van de muispositie
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Methode om de positie van de speler te resetten
    public void ResetPosition()
    {
        transform.position = new Vector2(0, 0); // Zet de positie van de speler op (0,0)
        rb.velocity = Vector2.zero; // Reset de snelheid van de Rigidbody
        rb.angularVelocity = 0; // Reset de hoeksnelheid van de Rigidbody
        rb.isKinematic = false; // Zorg ervoor dat de Rigidbody niet kinematisch is
        ResetMovement();
    }

    // Methode om de beweging van de speler te resetten
    public void ResetMovement()
    {
        movement = Vector2.zero; // Zet de beweging vector op 0
        animator.SetBool("IsWalking", false); // Zet de IsWalking parameter van de animator op false
    }

    // FixedUpdate wordt op vaste tijdsintervallen aangeroepen
    private void FixedUpdate()
    {
        // Beperk de beweging van de speler binnen bepaalde grenzen
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

        OnMouseMovement(); // Roep de methode aan om de speler naar de muis te draaien
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime); // Beweeg de speler
        rb.freezeRotation = true; // Bevries de rotatie van de Rigidbody
    }
}