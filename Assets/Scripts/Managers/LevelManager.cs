using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Life playerLife;
    [SerializeField] GameObject failLevelUI;
    [SerializeField] GameObject winLevelUI;
    [SerializeField] GameObject hudUI;
    static LevelManager instance;
    bool gameOver;

    int totalOfCitizenGroups;

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        MoveCitizens[] citizensGroups = GameObject.Find("Citizens").GetComponentsInChildren<MoveCitizens>();

        foreach (MoveCitizens citizenGroup in citizensGroups)
            citizenGroup.OnAllRescued.AddListener(WinLevel);

        totalOfCitizenGroups = citizensGroups.Length;

        playerLife.OnDeath.AddListener(FailLevel);
    }

    void FailLevel()
    {
        if (!gameOver)
        {
            gameOver = true;
            failLevelUI.SetActive(true);
            hudUI.SetActive(false);
        }
    }

    void WinLevel()
    {
        totalOfCitizenGroups--;
        Debug.Log(totalOfCitizenGroups);

        if (totalOfCitizenGroups <= 0 && !gameOver)
        {
            gameOver = true;
            winLevelUI.SetActive(true);
            hudUI.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        #if UNITY_STANDALONE
            GameManager.Instance.HideCursor();
        #endif
        GameManager.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        GameManager.Instance.FadeToScene(0);
    }

    public static LevelManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<LevelManager>();
            if (!instance)
            {
                GameObject gameObj = new GameObject("Level Manager");
                instance = gameObj.AddComponent<LevelManager>();
            }
            return instance;
        }
    }

    public bool GameOver
    {
        get { return gameOver; }
    }
}