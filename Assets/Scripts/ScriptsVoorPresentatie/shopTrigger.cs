using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    // Een boolean om te zien of de winkel actief is
    public static bool IsShopActive = false;

    // canvas dat de winkel weergeeft
    public GameObject shopCanvas;

    // WaveSpawner Script
    public WaveSpawner waveSpawner;

    // Deze functie wordt aangeroepen wanneer het object wordt geïnitialiseerd
    private void Awake()
    {
        // het winkelcanvas uitzetten bij het opstarten
        shopCanvas.SetActive(false);
    }

    // Deze functie wordt elke frame aangeroepen
    private void Update()
    {
        // Controleert of het spel is gepauzeerd door de GameIsPaused functie te checken in de PauseMenu script
        if (PauseMenu.GameIsPaused)
        {
            // deactiveer het winkelcanvas als het spel is gepauzeerd
            shopCanvas.SetActive(false);

            // resume spawnen van golven
            waveSpawner.ResumeSpawning();
            return;
        }
    }

    // Deze functie wordt aangeroepen wanneer er iets in de collider gaat
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controleert of de object de tag "Player" heeft
        if (collision.CompareTag("Player"))
        {
            // activeer het winkelcanvas
            shopCanvas.SetActive(true);

            // Pauzeer het spawnen van golven
            waveSpawner.PauseSpawning();

            // zet de winkelstatus op actief
            IsShopActive = true;
        }
    }

    // Deze functie wordt aangeroepen wanneer er iets de collider gebied uit gaat
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Controleert of de object de tag "Player" heeft
        if (collision.CompareTag("Player"))
        {
            // deactiveer het winkelcanvas
            shopCanvas.SetActive(false);

            // Hervat het spawnen van golven
            waveSpawner.ResumeSpawning();

            // zet de winkelstatus op niet actief
            IsShopActive = false;
        }
    }
}
