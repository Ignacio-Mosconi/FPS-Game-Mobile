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
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        #endif
        StartCoroutine("LoadFirstLevelAsync");
	}
	
	public void PlayGame()
    {
        #if UNITY_STANDALONE
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        #endif
        hasPressedPlay = true;
	}

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadFirstLevelAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstLevelName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (hasPressedPlay)
                asyncLoad.allowSceneActivation = true;
            yield return null;
        }
    }
}
