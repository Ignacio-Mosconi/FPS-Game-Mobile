using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] SettingsMenu settingsMenu;
    
    static GameManager instance;

    const float MIN_LOAD_TIME = 1f;

    SettingsMenu.GfxSetting currentGfxSetting;
    float currentSfxVolume;
    float currentMusicVolume;
    Animator animator;
    Slider loadingBarSlider;
    TextMeshProUGUI loadingText;
    int nextSceneToLoad = -1;

    void Awake()
    {
        if (Instance == this)
        {
            animator = GetComponent<Animator>();
#if UNITY_ANDROID
            currentGfxSetting = SettingsMenu.GfxSetting.Medium;
#endif
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
#if UNITY_STANDALONE
        currentGfxSetting = (SettingsMenu.GfxSetting)(PlayerPrefs.GetInt("GfxSetting", (int)SettingsMenu.GfxSetting.High));
#else
        currentGfxSetting = (SettingsMenu.GfxSetting)(PlayerPrefs.GetInt("GfxSetting", (int)SettingsMenu.GfxSetting.Medium));
#endif
        currentSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        if (settingsMenu)
        {
            settingsMenu.UpdateGraphicsSetting();
            settingsMenu.UpdateSfxVolume();
            settingsMenu.UpdateMusicVolume();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        loadingBarSlider = loadingScreen.GetComponentInChildren<Slider>();
        loadingText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
    }

    IEnumerator LoadSceneAsynchronously(int nextSceneToLoad)
    {
        if (nextSceneToLoad >= 0)
        {
            loadingScreen.SetActive(true);

            float loadTimer = 0f;
            float maxProgressValue = 0.9f + MIN_LOAD_TIME;
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneToLoad);

            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01((operation.progress + loadTimer) / maxProgressValue);
                loadingBarSlider.value = progress;
                loadingText.text = "Loading: " + (int)(progress * 100) + "%";
                loadTimer += Time.deltaTime;

                if (progress == 1f)
                    operation.allowSceneActivation = true;

                yield return null;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator.SetTrigger("Fade In");
        loadingScreen.SetActive(false);
        nextSceneToLoad = -1;
    }

    public void FadeToScene(int sceneIndex)
	{
		nextSceneToLoad = sceneIndex;
		animator.SetTrigger("Fade Out");
	}

	public void OnFadeOutComplete()
	{
		StartCoroutine(LoadSceneAsynchronously(nextSceneToLoad));
	}

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitApplication()
    {
        Application.Quit();
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

    public SettingsMenu.GfxSetting CurrentGfxSetting
    {
        get { return currentGfxSetting; }
        set 
        { 
            currentGfxSetting = value;
            PlayerPrefs.SetInt("GfxSetting", (int)currentGfxSetting);
        }
    }

    public float CurrentSfxVolume
    {
        get { return currentSfxVolume; }
        set 
        {
            currentSfxVolume = value;
            PlayerPrefs.SetFloat("SfxVolume", currentSfxVolume);
        }
    }
    public float CurrentMusicVolume
    {
        get { return currentMusicVolume; }
        set 
        {
            currentMusicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        }
    }
}