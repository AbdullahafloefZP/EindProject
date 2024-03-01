using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    public Text ammoDisplay;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform MuzzleFlash;
    [SerializeField] private int maxAmmo;
    private int currentAmmo;
    private int currentReserve;
    [SerializeField] private int maxReserve;

    private bool isReloading = false;
    public GameObject reloadMessage;
    public GameObject ammoMessage;
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
        currentReserve = maxReserve;
        reloadMessage.SetActive(false);
        ammoMessage.SetActive(false);
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    private void Update()
    {

        ammoDisplay.text = $"{currentAmmo}/{maxAmmo} | {currentReserve}/{maxReserve}";

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0 && currentReserve > 0)
        {
            StartCoroutine(Reload());
            reloadMessage.SetActive(true);
            return;
        } else if (Input.GetKeyDown(KeyCode.R) && currentReserve > 0)
        {
            StartCoroutine(Reload());
            reloadMessage.SetActive(true);
            return;
        }

        if (currentAmmo <=0 && currentReserve <=0)
        {
            MuzzleFlash.gameObject.SetActive(false);
            shootPoint.gameObject.SetActive(false);
            ammoMessage.SetActive(true);
            return;
        }
        else
        {
            ammoMessage.SetActive(false);
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

        animator.SetBool("Reloading", true);
        MuzzleFlash.gameObject.SetActive(false);
        shootPoint.gameObject.SetActive(false);

        yield return new WaitForSeconds(reloadTime);

        int bulletsToReload = Mathf.Min(maxAmmo - currentAmmo, currentReserve);
        currentAmmo += bulletsToReload;
        currentReserve -= bulletsToReload;

        animator.SetBool("Reloading", false);
        reloadMessage.SetActive(false);

        isShooting = false;
        isReloading = false;
    }

    private void Shoot()
    {
        lastShootTime = Time.time;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(gunTransform.right);
        }

        MuzzleFlash.gameObject.SetActive(true);
        Invoke("DisableMuzzleFlash", 0.1f);
    }

    private void DisableMuzzleFlash()
    {
        MuzzleFlash.gameObject.SetActive(false);
    }
    
    public void FlipCharacter(bool isFlipped)
    {
        flipped = isFlipped;
    }
}
