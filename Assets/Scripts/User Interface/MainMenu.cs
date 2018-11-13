using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] string firstLevelName;

    void OnEnable()
    {
#if UNITY_STANDALONE
        Invoke("Init", 0.1f);
#endif
    }

    void Init()
    {
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
