using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Statische boolean om bij te houden of het spel is gepauzeerd
    public static bool GameIsPaused = false;
    // Verwijzing naar het pauzemenu UI object
    public GameObject pauseMenuUI;
    // Verwijzing naar het verliesmenu script
    public LoseMenu loseMenu;
    // Verwijzing naar de PlayerHealth script
    public PlayerHealth playerHealth;

    // Deze functie wordt elke frame aangeroepen
    void Update()
    {
        // Controleer of de Escape-toets is ingedrukt
        if (Input.GetKeyDown(KeyCode.Escape))// Als het GameIsPaused boolean geroepen wordt(staat nu op false)...
        {
            if (GameIsPaused)
            {
                Resume(); //hervat het spel
            }
            else // Als het GameIsPaused boolean true is, pauzeer het spel
            {
                if (loseMenu == null || !loseMenu.loseMenuUI.activeInHierarchy) // als het losemenu niet active is kan je de game pauzeren. Als de losemenu wel actief is dan kan je niet pauzeren
                {
                    Pause();
                }
            }
        }
    }

    // Functie om het spel te pauzeren
    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Toon het pauzemenu canvas
        Time.timeScale = 0f; // Zet de tijdschaal op 0 om het spel te pauzeren
        GameIsPaused = true; // Stel de pauzestatus in op true
    }

    // Functie om het spel te hervatten
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Verberg het pauzemenu canvas
        Time.timeScale = 1f; // Zet de tijdschaal terug naar 1 om het spel te hervatten
        GameIsPaused = false; // Stel de pauzestatus in op false
    }

    // Functie om het spel te verlaten
    public void QuitGame()
    {
        playerHealth.SaveLives(); // Sla het aantal levens van de speler op
        Application.Quit(); // Sluit de applicatie
    }
}