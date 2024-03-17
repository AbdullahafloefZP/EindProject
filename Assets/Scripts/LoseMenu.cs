using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public static bool PlayerHasDied = false;
    public GameObject loseMenuUI;

    void Start()
    {
        loseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (PlayerHasDied)
        {
            Lose();
            PlayerHasDied = false;
        }
    }

    void Lose()
    {
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        // EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
