using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Settings Values")]
    [SerializeField] float volumeSliderValue;
    static GameManager instance;
    EventSystem eventSystem;

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
            inputModule.verticalAxis = "Vertical UI";
            inputModule.submitButton = "Select";
            inputModule.cancelButton = "Return";
        }
        DontDestroyOnLoad(eventSystem.gameObject);
    }

    public void ChangeFirstMenuItemSelected(GameObject firstMenuElement)
    {
        eventSystem.firstSelectedGameObject = firstMenuElement;
        eventSystem.SetSelectedGameObject(firstMenuElement);
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

    public float VolumeSliderValue
    {
        get { return volumeSliderValue; }
        set { volumeSliderValue = value; }
    }
}

