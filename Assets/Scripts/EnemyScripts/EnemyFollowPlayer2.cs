using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer2 : MonoBehaviour
{
    private GameObject pooledBullet;
    private List<GameObject> pooledBullets = new List<GameObject>();
    private int pooledAmount = 1;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform MuzzleFlash;
    [SerializeField] private int maxAmmo;
    private int currentAmmo;
    private bool isReloading = false;
    public GameObject reloadMessage;
    [SerializeField] private float reloadTime = 1f;
    public Animator ejectedBulletAnimator;
    public float shootRate = 0.2f;
    public int damages = 1;
    private bool isShooting = false;
    private bool flipped = false;
    private float lastShootTime = 0f;
    public GameObject ejectedBulletPrefab;
    public Transform EjectPoint;
    private Transform weaponsParent;

    [SerializeField] private float speed;
    [SerializeField] private float lineOfSight;
    [SerializeField] private float shootingRange;
    [SerializeField] private GameObject Bullet;
    private Transform player;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponsParent = transform.Find("EnemyWeapon");

        shootPoint.gameObject.SetActive(false);
        gunTransform.gameObject.SetActive(false);
        currentAmmo = maxAmmo;
        reloadMessage.SetActive(false);

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject bullet = Instantiate(ejectedBulletPrefab);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }

    private void Update()
    {
        UpdateGunRotation();
    
        if (currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
            return;
        }
    
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        Vector2 moveDirection;
    
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > shootingRange)
        {
            moveDirection = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange)
        {
            isShooting = true;
            shootPoint.gameObject.SetActive(true);
            gunTransform.gameObject.SetActive(true);
        }
    
        if (distanceFromPlayer < lineOfSight)
        {
            moveDirection = (player.position - transform.position).normalized;
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    
        SetAnimationParameters(moveDirection);
    
        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                Vector3 aimDirection = (player.position - child.position).normalized;
                float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    
                child.eulerAngles = new Vector3(0, 0, angle);
                child.eulerAngles = new Vector3(0, 0, angle);
            }
        }
    
        if (isReloading)
        {
            // Ensure shootPoint is deactivated while reloading
            shootPoint.gameObject.SetActive(false);
            return;
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
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    private void UpdateGunRotation()
    {
        Vector3 direction = player.position - transform.position;

        foreach (Transform child in weaponsParent)
        {
            if (child != null)
            {
                SpriteRenderer gunSpriteRenderer = child.GetComponent<SpriteRenderer>();

                if (direction.x > 0)
                {
                    child.localScale = new Vector3(-Mathf.Abs(child.localScale.x), child.localScale.y, child.localScale.z);
                    child.localScale = new Vector3(child.localScale.x, Mathf.Abs(child.localScale.y), child.localScale.z);
                }
                else if (direction.x < 0)
                {
                    child.localScale = new Vector3(-Mathf.Abs(child.localScale.x), child.localScale.y, child.localScale.z);
                    child.localScale = new Vector3(child.localScale.x, -Mathf.Abs(child.localScale.y), child.localScale.z);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        rb.freezeRotation = true;
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
    }

    private IEnumerator Reload()
{
    isReloading = true;

    MuzzleFlash.gameObject.SetActive(false);
    shootPoint.gameObject.SetActive(false);
    reloadMessage.SetActive(true);

    yield return new WaitForSeconds(reloadTime);

    int bulletsToReload = Mathf.Min(maxAmmo - currentAmmo);
    currentAmmo += bulletsToReload;

    currentAmmo = maxAmmo;
    reloadMessage.SetActive(false);
    
    shootPoint.gameObject.SetActive(false);

    isShooting = false;
    isReloading = false;
}



    private void Shoot()
    {
        GameObject ejectedBullet = GetPooledBullet();
        ejectedBullet.transform.SetParent(EjectPoint);
        ejectedBullet.transform.position = EjectPoint.position;
        ejectedBullet.transform.rotation = EjectPoint.rotation;
        ejectedBullet.SetActive(true);

        lastShootTime = Time.time;

        Vector2 directionToPlayer = (player.position - gunTransform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, directionToPlayer);

        if (hit && hit.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damages);
            }
        }

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(directionToPlayer);
        }

        MuzzleFlash.gameObject.SetActive(true);
        Invoke("DisableMuzzleFlash", 0.1f);

        currentAmmo--;
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
