using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject hudUI;
    [SerializeField] GameObject firstMenuElement;
    static bool isPaused = false;

    void Update()
    {
        if (InputManager.Instance.GetPauseButton() && !LevelManager.Instance.GameOver)
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        hudUI.SetActive(false);
        Time.timeScale = 0.0f;
        isPaused = true;
        #if UNITY_STANDALONE
            if (InputManager.Instance.CheckControllerConnection())
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
            else
                GameManager.Instance.ShowCursor();
        #endif
    }

    public void Resume()
    {
        #if UNITY_STANDALONE
            GameManager.Instance.HideCursor();
        #endif
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
        InputManager.Instance.ChangeFirstMenuItemSelected(null);
    }

    public void LoadMenu()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        GameManager.Instance.FadeToScene(0);
    }

    public static bool IsPaused
    {
        get { return isPaused; }
    }
}
