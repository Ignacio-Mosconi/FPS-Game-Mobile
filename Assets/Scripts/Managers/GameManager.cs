using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings Values")]
    [SerializeField] float volumeSliderValue;
    static GameManager instance;

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

    public float VolumeSliderValue
    {
        get { return volumeSliderValue; }
        set { volumeSliderValue = value; }
    }
}

