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