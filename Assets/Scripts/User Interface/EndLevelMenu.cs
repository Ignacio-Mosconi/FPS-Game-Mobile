using UnityEngine;
using UnityEngine.Events;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;
    
    UnityEvent onMenuToggle = new UnityEvent();

    void Start()
    {
        Time.timeScale = 0f;
        onMenuToggle.Invoke();
        if (InputManager.Instance.CheckControllerConnection())
            InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        LevelManager.Instance.RestartLevel();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        LevelManager.Instance.LoadMenu();
    }

    public UnityEvent OnMenuToggle
    {
        get { return onMenuToggle; }
    }
}
