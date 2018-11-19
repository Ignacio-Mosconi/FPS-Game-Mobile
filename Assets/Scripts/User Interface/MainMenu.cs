using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] string firstLevelName;

#if UNITY_STANDALONE
    bool wasControllerConnected = false;
    bool wasInitialized = false;
#endif

#if UNITY_STANDALONE
    void OnEnable()
    {
        wasControllerConnected = false;
        wasInitialized = false;
        Invoke("Initialize", 0.1f);
    }

    void Update()
    {
        if (wasInitialized)
            HandleControllerConnection();
    }

    void Initialize()
    {
        wasInitialized = true;
    }

    void HandleControllerConnection()
    {
        bool isConnected = InputManager.Instance.CheckControllerConnection();

        if (isConnected)
        {
            if (!wasControllerConnected)
            {
                GameManager.Instance.HideCursor();
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
                wasControllerConnected = true;
            }
        }
        else
            if (wasControllerConnected)
            {
                GameManager.Instance.ShowCursor();
                InputManager.Instance.ChangeFirstMenuItemSelected(null);
                wasControllerConnected = false;
            }
    }
#endif
	
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
