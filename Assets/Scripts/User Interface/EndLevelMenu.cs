using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;

    void Start()
    {
        Time.timeScale = 0.0f;
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
        Time.timeScale = 1.0f;
        LevelManager.Instance.RestartLevel();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        LevelManager.Instance.LoadMenu();
    }
}
