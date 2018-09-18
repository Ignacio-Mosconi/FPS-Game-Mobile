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
        if (GameManager.Instance.CheckControllerConnection())
            GameManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
        GameManager.Instance.ChangeFirstMenuItemSelected(null);
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
