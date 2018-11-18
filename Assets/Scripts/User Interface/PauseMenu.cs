using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject hudUI;
    [SerializeField] GameObject mobileControls;
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] Animator pauseMenuAnimator;
    [SerializeField] AnimationClip fadeOutAnimation;
    
    static bool isPaused = false;

    UnityEvent onPauseToggle = new UnityEvent();

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

    void Continue()
    {
#if UNITY_STANDALONE
        GameManager.Instance.HideCursor();
#else
        mobileControls.SetActive(true);
#endif
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(true);
        isPaused = false;
        onPauseToggle.Invoke();
        InputManager.Instance.ChangeFirstMenuItemSelected(null);
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
        Time.timeScale = 1f;
        pauseMenuAnimator.SetTrigger("Fade Out");
        Invoke("Continue", fadeOutAnimation.length);
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
