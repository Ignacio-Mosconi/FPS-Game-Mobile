using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject firstMenuElement;

	void Start()
    {
        if (GameManager.Instance.CheckControllerConnection())
            GameManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
	}
	
	public void PlayGame()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void Quit()
    {
        Application.Quit();
    }
}
