using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] string firstLevelName;
    bool enabledInitialization = false;

    void OnEnable()
    {
#if UNITY_STANDALONE
        Invoke("Init", 0.1f);
#endif
    }

    void Init()
    {
        enabledInitialization = true;
        if (InputManager.Instance.CheckControllerConnection())
        {
            GameManager.Instance.HideCursor();
            InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
        }
        else
            GameManager.Instance.ShowCursor();
    }
	
	public void PlayGame()
    {
#if UNITY_STANDALONE
        GameManager.Instance.HideCursor();
#endif
        GameManager.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void Quit()
    {
        GameManager.Instance.QuitApplication();
    }
}
