using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject hudUI;
    static bool isPaused = false;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        if (!LevelManager.Instance.GameOver)
        {
            Cursor.visible = true;
            pauseMenuUI.SetActive(true);
            hudUI.SetActive(false);
            Time.timeScale = 0.0f;
            isPaused = true;
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void LoadMenu()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public static bool IsPaused
    {
        get { return isPaused; }
    }
}
