using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;

    void Start()
    {
        Time.timeScale = 0.0f;
        if (GameManager.Instance.CheckControllerConnection())
            GameManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
        else
            Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        LevelManager.Instance.RestartLevel();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
