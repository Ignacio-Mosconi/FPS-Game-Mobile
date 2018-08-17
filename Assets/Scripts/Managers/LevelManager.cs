﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Life playerLife;
    [SerializeField] GameObject failLevelUI;
    [SerializeField] GameObject hudUI;
    static LevelManager instance;
    bool gameOver;

    void Awake()
    {
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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
