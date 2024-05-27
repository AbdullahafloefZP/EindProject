using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour
{
    // Variabelen voor kogels en hun beheer
    private GameObject pooledBullet;
    private List<GameObject> pooledBullets = new List<GameObject>();
    private int pooledAmount = 1; // Aantal vooraf gepoolde kogels

    // UI-elementen voor ammunitieweergave
    public Text ammoDisplay;
    public CursorManager cursorManager;

    // Transforms en prefab voor geweer en schietpunt
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform MuzzleFlash;

    // Ammunitie-instellingen
    [SerializeField] private int maxAmmo;
    private int currentAmmo;
    private int currentReserve;
    [SerializeField] private int maxReserve;

    // Reloading en schietinstellingen
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
    public AudioClip shootingSound;
    AudioManager audioManager;

    // Initialisatie van variabelen en het voorbereiden van kogels
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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

    // Haalt een gepoolde kogel op of maakt er een nieuwe als er geen beschikbaar is
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

    // Herstel de reload-status bij het inschakelen van het object
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update-functie die elke frame wordt aangeroepen
    private void Update()
    {
        // Controleer of het spel is gepauzeerd of de winkel actief is
        if (PauseMenu.GameIsPaused || ShopTrigger.IsShopActive)
        {
            return;
        }

        // geef de ammo weer op het scherm
        ammoDisplay.text = $"{currentAmmo}/{maxAmmo} | {currentReserve}/{maxReserve}";
    
        // Controleer of er herladen wordt
        if (isReloading)
        {
            ammoMessage.SetActive(false);
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
    
        // Controleer of er geschoten moet worden
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
            
            if (Input.GetMouseButton(0) && !cursorManager.IsPointerOverUI())
            {
                Shoot();
            }
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

            if (Input.GetMouseButton(0) && !cursorManager.IsPointerOverUI())
            {
                Shoot();
            }
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

    // Coroutine om het herladen van het wapen te beheren
    private IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);
        MuzzleFlash.gameObject.SetActive(false);
        shootPoint.gameObject.SetActive(false);
        audioManager.PlaySoundReload(audioManager.reloading);

        yield return new WaitForSeconds(reloadTime);

        int bulletsToReload = Mathf.Min(maxAmmo - currentAmmo, currentReserve);
        currentAmmo += bulletsToReload;
        currentReserve -= bulletsToReload;

        animator.SetBool("Reloading", false);
        reloadMessage.SetActive(false);

        isShooting = false;
        isReloading = false;
    }

    // Functie om te schieten
    private void Shoot()
    {
        GameObject ejectedBullet = GetPooledBullet();
        ejectedBullet.transform.SetParent(EjectPoint);
        ejectedBullet.transform.position = EjectPoint.position;
        ejectedBullet.transform.rotation = EjectPoint.rotation;
        ejectedBullet.SetActive(true);

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = gunTransform.right * (flipped ? -1 : 1);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(direction);
            bulletComponent.damage = damages;
        }

        MuzzleFlash.gameObject.SetActive(true);
        Invoke("DisableMuzzleFlash", 0.1f);

        audioManager.PlaySoundReload(shootingSound);

        currentAmmo--;
        lastShootTime = Time.time;

        if (transform.localScale.x > 0)
        {
            MuzzleFlash.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (transform.localScale.x < 0)
        {
            MuzzleFlash.localEulerAngles = new Vector3(0, 0, 270);
        }
    }

    // Functie om de herlaadstatus te resetten
    public void ResetReloadingState()
    {
        isReloading = false;
        reloadMessage.SetActive(false);
        animator.SetBool("Reloading", false);
    }

    // Functie om de ammunitie te verversen
    public void RefreshAmmo()
    {
        currentReserve = maxReserve;
        UpdateAmmoUI();
    }

    // Functie om de ammunitie te resetten
    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
        currentReserve = maxReserve;
        UpdateAmmoUI();
    }

    // Controleer of de ammunitie kan worden ververst
    public bool CanRefreshAmmo()
    {
        return currentReserve < maxReserve;
    }

    // Functie om de ammunitie UI bij te werken
    private void UpdateAmmoUI()
    {
        ammoDisplay.text = $"{currentAmmo}/{maxAmmo} | {currentReserve}/{maxReserve}";
    }

    // Functie om de Muzzle Flash uit te schakelen
    private void DisableMuzzleFlash()
    {
        MuzzleFlash.gameObject.SetActive(false);
    }

    // Functie om de character te draaien
    public void FlipCharacter(bool isFlipped)
    {
        flipped = isFlipped;
    }
}