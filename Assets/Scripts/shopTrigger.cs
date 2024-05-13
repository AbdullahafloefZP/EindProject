using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public static bool IsShopActive = false;
    public GameObject shopCanvas;
    public WaveSpawner waveSpawner;

    private void Awake()
    {
        shopCanvas.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            shopCanvas.SetActive(false);
            waveSpawner.ResumeSpawning();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopCanvas.SetActive(true);
            waveSpawner.PauseSpawning();
            IsShopActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shopCanvas.SetActive(false);
            waveSpawner.ResumeSpawning();
            IsShopActive = false;
        }
    }
}
