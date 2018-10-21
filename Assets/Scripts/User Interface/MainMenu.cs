using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] string firstLevelName;
    bool hasPressedPlay = false;

	void Start()
    {
        #if UNITY_STANDALONE
            if (InputManager.Instance.CheckControllerConnection())
            {
                GameManager.Instance.HideCursor();
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
            }
            else
                GameManager.Instance.ShowCursor();
        #endif
        //StartCoroutine("LoadFirstLevelAsync");
	}
	
	public void PlayGame()
    {
        #if UNITY_STANDALONE
            GameManager.Instance.HideCursor();
        #endif
        GameManager.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
        // #if UNITY_STANDALONE
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // #endif
        // hasPressedPlay = true;
	}

    public void Quit()
    {
        GameManager.Instance.QuitApplication();
    }

    // IEnumerator LoadFirstLevelAsync()
    // {
    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstLevelName);
    //     asyncLoad.allowSceneActivation = false;
    //     while (!asyncLoad.isDone)
    //     {
    //         if (hasPressedPlay)
    //             asyncLoad.allowSceneActivation = true;
    //         yield return null;
    //     }
    // }
}
