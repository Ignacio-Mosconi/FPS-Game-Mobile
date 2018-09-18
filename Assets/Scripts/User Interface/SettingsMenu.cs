using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioMixer sfxMixer;
    [SerializeField] AudioMixer musicMixer;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] GameObject[] sliderButtons;

    void OnEnable()
    {
        sfxVolumeSlider.value = GameManager.Instance.SfxVolumeValue;
        musicVolumeSlider.value = GameManager.Instance.MusicVolumeValue;

        foreach (GameObject sliderButton in sliderButtons)
            sliderButton.SetActive(GameManager.Instance.CheckControllerConnection());
    }

    float GetMixerLevel(AudioMixer audioMixer)
    {
        float volume;
        bool result = audioMixer.GetFloat("Volume", out volume);
        return (result) ? volume : 0f;
    }

    public void SetSfxVolume(float volume)
    {
        GameManager.Instance.SfxVolumeValue = volume;
        sfxMixer.SetFloat("Volume", Mathf.Log(volume) * 12);
    }
    public void SetMusicVolume(float volume)
    {
        GameManager.Instance.MusicVolumeValue = volume;
        musicMixer.SetFloat("Volume", Mathf.Log(volume) * 12);
    }

    public void IncreaseSliderValue(Slider slider)
    {
        slider.value += 0.1f;
    }

    public void DecreaseSliderValue(Slider slider)
    {
        slider.value -= 0.1f;
    }
}