using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    EventSystem eventSystem;
    float sfxVolumeValue = 0.75f;
    float musicVolumeValue = 0.75f;

    void Awake()
    {
        if (Instance == this)
            DontDestroyOnLoad(gameObject);
       
        eventSystem = FindObjectOfType<EventSystem>();
        if (!eventSystem)
        {
            GameObject gameObj = new GameObject("EventSystem");
            eventSystem = gameObj.AddComponent<EventSystem>();
            StandaloneInputModule inputModule = gameObj.AddComponent<StandaloneInputModule>();
            #if UNITY_ANDROID
            if (CheckControllerConnection())
            {
                inputModule.verticalAxis = "Vertical UI Controller Mobile";
                inputModule.submitButton = "Select Controller Mobile";
                inputModule.cancelButton = "Return Controller Mobile";
            }
            else
            {
                inputModule.verticalAxis = "Vertical UI";
                inputModule.horizontalAxis = "Horizontal UI";
                inputModule.submitButton = "Select";
                inputModule.cancelButton = "Return";
            }
            #else
                inputModule.verticalAxis = "Vertical UI";
                inputModule.horizontalAxis = "Horizontal UI";
                inputModule.submitButton = "Select";
                inputModule.cancelButton = "Return";
            #endif
        }
        DontDestroyOnLoad(eventSystem.gameObject);
    }

    public void ChangeFirstMenuItemSelected(GameObject firstMenuElement)
    {
        if (CheckControllerConnection())
        {
            eventSystem.firstSelectedGameObject = firstMenuElement;
            eventSystem.SetSelectedGameObject(firstMenuElement);
        }
    }

    public bool CheckControllerConnection()
    {
        bool controllerConnected = false;
        string[] controllers = Input.GetJoystickNames();
        
        for (int i = 0; i < controllers.Length; i++)
        {
            if (!string.IsNullOrEmpty(controllers[i]))
            {
                controllerConnected = true;
                break;
            }
        }

        return controllerConnected;
    }

    public static GameManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameManager>();
            if (!instance)
            {
                GameObject gameObj = new GameObject("Game Manager");
                instance = gameObj.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public float SfxVolumeValue
    {
        get { return sfxVolumeValue; }
        set { sfxVolumeValue = value; }
    }
    public float MusicVolumeValue
    {
        get { return musicVolumeValue; }
        set { musicVolumeValue = value; }
    }
}