using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject hudUI;
    [SerializeField] GameObject mobileControls;
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] UnityEvent onPauseToggle;
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

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        hudUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        onPauseToggle.Invoke();
        #if UNITY_STANDALONE
            if (InputManager.Instance.CheckControllerConnection())
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
            else
                GameManager.Instance.ShowCursor();
        #else
            mobileControls.SetActive(false);
        #endif
    }

    public void Resume()
    {
        #if UNITY_STANDALONE
            GameManager.Instance.HideCursor();
        #else
            mobileControls.SetActive(true);
        #endif
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        onPauseToggle.Invoke();
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

    public UnityEvent OnPauseToggle
    {
        get { return onPauseToggle; }
    }
}
