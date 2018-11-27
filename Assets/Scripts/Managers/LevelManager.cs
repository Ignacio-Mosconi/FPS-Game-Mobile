using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Life playerLife;
    [SerializeField] GameObject failLevelUI;
    [SerializeField] GameObject winLevelUI;
    [SerializeField] GameObject hudUI;
    [SerializeField] AudioSource music;
    
    float MUSIC_START_DELAY = 15f;
    
    static LevelManager instance;

    bool gameOver;
    int totalOfCitizenGroups;
    float musicTimer = 0f;

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

        ZombieAI.OnFirstPlayerDetection.AddListener(NotifyFirstZombieDetection);

        totalOfCitizenGroups = citizensGroups.Length;

        playerLife.OnDeath.AddListener(FailLevel);
    }

    void Update()
    {
        if (ZombieAI.FirstPlayerDetection && !music.isPlaying && !PauseMenu.IsPaused)
        {
            musicTimer += Time.deltaTime;
            
            if (musicTimer >= MUSIC_START_DELAY)
            {
                musicTimer = 0f;
                music.Play();
            }
        }
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

        if (totalOfCitizenGroups <= 0 && !gameOver)
        {
            gameOver = true;
            winLevelUI.SetActive(true);
            hudUI.SetActive(false);
        }
    }

    void NotifyFirstZombieDetection()
    {
        if (!ZombieAI.FirstPlayerDetection)
            ZombieAI.FirstPlayerDetection = true;
        ZombieAI.OnFirstPlayerDetection.RemoveListener(NotifyFirstZombieDetection);

        music.Play();
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