using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public enum GfxSetting
    {
        Low, Medium, High
    }

    [Header("Graphics Settings")]
    [SerializeField] TextMeshProUGUI gfxText;
    [SerializeField] Button increaseGfxButton;
    [SerializeField] Button decreaseGfxButton;

    [Header("Audio Settings")]
    [SerializeField] AudioMixer sfxMixer;
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] GameObject[] sliderButtons;

    [Header("UI Navigation")]
    [SerializeField] GameObject firstMenuElement;
    [SerializeField] Button[] controllerNavigationButtons;

    const float MIXER_MULT = 20f;
    const float SLIDER_INCREMENT_VALUE = 0.1f;

#if UNITY_STANDALONE
    AudioSource selectionSound;
#endif
    bool wasControllerConnected;

#if UNITY_STANDALONE
    void Awake()
    {
        selectionSound = GetComponentInParent<AudioSource>();
    }
#endif

    void OnEnable()
    {
        wasControllerConnected = false;
        gfxText.text = GameManager.Instance.CurrentGfxSetting.ToString();
        if (GameManager.Instance.CurrentGfxSetting == GfxSetting.High)
            increaseGfxButton.interactable = false;
        if (GameManager.Instance.CurrentGfxSetting == GfxSetting.Low)
            decreaseGfxButton.interactable = false;
        sfxVolumeSlider.value = GameManager.Instance.CurrentSfxVolume;
        musicVolumeSlider.value = GameManager.Instance.CurrentMusicVolume;
        foreach (GameObject sliderButton in sliderButtons)
            sliderButton.SetActive(false);
        foreach (Button navigationButton in controllerNavigationButtons)
            navigationButton.interactable = false;
    }

#if UNITY_STANDALONE
    void Update()
    {
        HandleControllerConnection();

        if (InputManager.Instance.ControllerConnected)
        {
            if (InputManager.Instance.EventSystem.currentSelectedGameObject == controllerNavigationButtons[0].gameObject)
            {
                if (InputManager.Instance.GetRightUIButton())
                {
                    IncreaseGfxSetting();
                    selectionSound.Play();
                }
                if (InputManager.Instance.GetLeftUIButton())
                {
                    DecreaseGfxSetting();
                    selectionSound.Play();
                }
            }
            else
            {
                if (InputManager.Instance.EventSystem.currentSelectedGameObject == controllerNavigationButtons[1].gameObject)
                {
                    if (InputManager.Instance.GetRightUIButton())
                    {
                        IncreaseSliderValue(sfxVolumeSlider);
                        selectionSound.Play();
                    }
                    if (InputManager.Instance.GetLeftUIButton())
                    {
                        DecreaseSliderValue(sfxVolumeSlider);
                        selectionSound.Play();
                    }
                }
                else
                {
                    if (InputManager.Instance.EventSystem.currentSelectedGameObject == controllerNavigationButtons[2].gameObject)
                    {
                        if (InputManager.Instance.GetRightUIButton())
                        {
                            IncreaseSliderValue(musicVolumeSlider);
                            selectionSound.Play();
                        }
                        if (InputManager.Instance.GetLeftUIButton())
                        {
                            DecreaseSliderValue(musicVolumeSlider);
                            selectionSound.Play();
                        }
                    }
                }
            }
        }
    }
#endif

    void HandleControllerConnection()
    {
        bool isConnected = InputManager.Instance.CheckControllerConnection();

        if (isConnected)
        {
            if (!wasControllerConnected)
            {
                GameManager.Instance.HideCursor();
                InputManager.Instance.ChangeFirstMenuItemSelected(firstMenuElement);         
                foreach (GameObject sliderButton in sliderButtons)
                    sliderButton.SetActive(true);
                foreach (Button navigationButton in controllerNavigationButtons)
                    navigationButton.interactable = true;
                wasControllerConnected = true;
            }
        }
        else
            if (wasControllerConnected)
            {    
                GameManager.Instance.ShowCursor();
                InputManager.Instance.ChangeFirstMenuItemSelected(null);
                foreach (GameObject sliderButton in sliderButtons)
                    sliderButton.SetActive(false);
                foreach (Button navigationButton in controllerNavigationButtons)
                    navigationButton.interactable = false;
                wasControllerConnected = false;
            }
    }

    public void IncreaseGfxSetting()
    {
        if (GameManager.Instance.CurrentGfxSetting != GfxSetting.High)
        {
            GameManager.Instance.CurrentGfxSetting++;
            QualitySettings.SetQualityLevel((int)GameManager.Instance.CurrentGfxSetting);

            gfxText.text = GameManager.Instance.CurrentGfxSetting.ToString();

            if (GameManager.Instance.CurrentGfxSetting == GfxSetting.High)
                increaseGfxButton.interactable = false;
            if (!decreaseGfxButton.interactable)
                decreaseGfxButton.interactable = true;
        }
    }

    public void DecreaseGfxSetting()
    {
        if (GameManager.Instance.CurrentGfxSetting != GfxSetting.Low)
        {
            GameManager.Instance.CurrentGfxSetting--;
            QualitySettings.SetQualityLevel((int)GameManager.Instance.CurrentGfxSetting);

            gfxText.text = GameManager.Instance.CurrentGfxSetting.ToString();
            
            if (GameManager.Instance.CurrentGfxSetting == GfxSetting.Low)
                decreaseGfxButton.interactable = false;
            if (!increaseGfxButton.interactable)
                increaseGfxButton.interactable = true;
        }
    }

    public void SetSfxVolume(float volume)
    {
        GameManager.Instance.CurrentSfxVolume = volume;
        sfxMixer.SetFloat("Volume", Mathf.Log(volume) * MIXER_MULT);
    }
    
    public void SetMusicVolume(float volume)
    {
        GameManager.Instance.CurrentMusicVolume = volume;
        musicMixer.SetFloat("Volume", Mathf.Log(volume) * MIXER_MULT);
    }

    public void IncreaseSliderValue(Slider slider)
    {
        slider.value += SLIDER_INCREMENT_VALUE;
    }

    public void DecreaseSliderValue(Slider slider)
    {
        slider.value -= SLIDER_INCREMENT_VALUE;
    }

    public void UpdateGraphicsSetting()
    {
        QualitySettings.SetQualityLevel((int)GameManager.Instance.CurrentGfxSetting);
    }

    public void UpdateSfxVolume()
    {
        sfxMixer.SetFloat("Volume", Mathf.Log(GameManager.Instance.CurrentSfxVolume) * MIXER_MULT);
    }

    public void UpdateMusicVolume()
    {
        musicMixer.SetFloat("Volume", Mathf.Log(GameManager.Instance.CurrentMusicVolume) * MIXER_MULT);
    }
}