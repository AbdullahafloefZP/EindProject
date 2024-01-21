using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    public Text ammoDisplay;
    public LineRenderer lineRenderer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform MuzzleFlash;
    [SerializeField] private int maxAmmo = 15;
    private int currentAmmo;
    private bool isReloading = false;
    [SerializeField] private float reloadTime = 1f;

    public Animator animator;

    public float shootRate = 0.2f;
    public float damages = 1f;
    private bool isShooting = false;
    private bool flipped = false;
    private float lastShootTime = 0f;

    private void Awake() 
    {
        shootPoint.gameObject.SetActive(false);
        gunTransform.gameObject.SetActive(false);
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    private void Update()
    {

        ammoDisplay.text = currentAmmo.ToString();

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time - lastShootTime >= shootRate)
        {
            isShooting = true;
            shootPoint.gameObject.SetActive(true);
            gunTransform.gameObject.SetActive(true);

            if (transform.localScale.x > 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 90); 
            }
            else if (transform.localScale.x < 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 270); 
            }
            Shoot();
        }
        
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isShooting = false;
            lineRenderer.enabled = false;
            shootPoint.gameObject.SetActive(false);
            gunTransform.gameObject.SetActive(true);
        }

        if (isShooting && Time.time - lastShootTime >= shootRate)
        {
            isShooting = true;
            shootPoint.gameObject.SetActive(true);

            if (transform.localScale.x > 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 90); 
            }
            else if (transform.localScale.x < 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 270); 
            }
            Shoot();
        }

        if (isShooting && Time.time - lastShootTime >= 0.1f)
        {
            shootPoint.gameObject.SetActive(false);
            gunTransform.gameObject.SetActive(true);
            lineRenderer.enabled = false;

            if (transform.localScale.x > 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 90); 
            }
            else if (transform.localScale.x < 0)
            {
                MuzzleFlash.localEulerAngles = new Vector3(0, 0, 270); 
            }
        }
    }

    private IEnumerator Reload()
{
    isReloading = true;
    Debug.Log("Reloading..");

    animator.SetBool("Reloading", true);
    lineRenderer.enabled = false;
    MuzzleFlash.gameObject.SetActive(false);
    shootPoint.gameObject.SetActive(false);

    yield return new WaitForSeconds(reloadTime);

    animator.SetBool("Reloading", false);
    lineRenderer.enabled = true;
    MuzzleFlash.gameObject.SetActive(true);

    shootPoint.gameObject.SetActive(true);

    currentAmmo = maxAmmo;
    isReloading = false;
}


    private void Shoot()
    {
        lastShootTime = Time.time;
        lineRenderer.enabled = true;

        currentAmmo--;

        RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, gunTransform.right);

        if (hit)
        {
            lineRenderer.SetPosition(0, gunTransform.position);
            lineRenderer.SetPosition(1, hit.point);

            DamageFlash damageFlash = hit.collider.GetComponent<DamageFlash>();
            if (damageFlash != null)
            {
                damageFlash.CallDamageFlash();
                hit.collider.gameObject.TryGetComponent<DamageFlash>(out DamageFlash zombieComponent);
                if (zombieComponent != null)
                {
                    zombieComponent.TakeDamage(damages);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, gunTransform.position);
            Vector3 shootDirection = gunTransform.position + gunTransform.right * 100f;
            lineRenderer.SetPosition(1, shootDirection);
        }
    }
    public void FlipCharacter(bool isFlipped)
    {
        flipped = isFlipped;
    }
}
