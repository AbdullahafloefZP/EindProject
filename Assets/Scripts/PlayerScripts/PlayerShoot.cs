using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerShooting : MonoBehaviour
{
    private GameObject pooledBullet;
    private List<GameObject> pooledBullets = new List<GameObject>();
    private int pooledAmount = 1;
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
    public Animator ejectedBulletAnimator;
    public float shootRate = 0.2f;
    public float damages = 1f;
    private bool isShooting = false;
    private bool flipped = false;
    private float lastShootTime = 0f;
    public GameObject ejectedBulletPrefab;
    public Transform EjectPoint;

    private void Awake()
    {
        shootPoint.gameObject.SetActive(false);
        gunTransform.gameObject.SetActive(false);
        currentAmmo = maxAmmo;
        currentReserve = maxReserve;
        reloadMessage.SetActive(false);
        ammoMessage.SetActive(false);

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject bullet = Instantiate(ejectedBulletPrefab);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }

    private GameObject GetPooledBullet()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }

        GameObject newBullet = Instantiate(ejectedBulletPrefab);
        pooledBullets.Add(newBullet);
        return newBullet;
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
    }
    else if (Input.GetKeyDown(KeyCode.R) && currentReserve > 0)
    {
        StartCoroutine(Reload());
        reloadMessage.SetActive(true);
        return;
    }

    if (currentAmmo <= 0 && currentReserve <= 0)
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
        // ejectedBulletAnimator.SetBool("Shooting", true);

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
        // ejectedBulletAnimator.SetBool("Shooting", false);
    }

    if (isShooting && Time.time - lastShootTime >= shootRate)
    {
        isShooting = true;
        // ejectedBulletAnimator.SetBool("Shooting", true);
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
        // ejectedBulletAnimator.SetBool("Shooting", false);
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
        GameObject ejectedBullet = GetPooledBullet();
        ejectedBullet.transform.position = EjectPoint.position;
        ejectedBullet.transform.rotation = EjectPoint.rotation;
        ejectedBullet.transform.SetParent(EjectPoint);
        ejectedBullet.transform.localPosition = Vector3.zero;
        ejectedBullet.SetActive(true);

        Vector3 initialRelativePosition = transform.InverseTransformPoint(ejectedBullet.transform.position);

    // Remove ejected bullet's parent to prevent it from following player's movement

    // Start a coroutine to continually update the ejected bullet's position relative to the player
    StartCoroutine(UpdateRelativePosition(ejectedBullet, initialRelativePosition));
        lastShootTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, gunTransform.right);

        if (hit)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection((hit.point - (Vector2)shootPoint.position).normalized);
            }

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
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(gunTransform.right);
            }
        }

        MuzzleFlash.gameObject.SetActive(true);
        Invoke("DisableMuzzleFlash", 0.1f);

        currentAmmo--;
    }

    private IEnumerator UpdateRelativePosition(GameObject ejectedBullet, Vector3 initialRelativePosition)
{
    while (true)
    {
        // Calculate the player's position in world space
        Vector3 playerPosition = transform.TransformPoint(initialRelativePosition);

        // Update ejected bullet's position relative to the player
        ejectedBullet.transform.position = playerPosition;

        yield return null;
    }
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
