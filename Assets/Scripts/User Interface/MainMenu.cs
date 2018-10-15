using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;

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
	}
	
	public void PlayGame()
    {
        #if UNITY_STANDALONE
            Cursor.visible = false;
        #endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
