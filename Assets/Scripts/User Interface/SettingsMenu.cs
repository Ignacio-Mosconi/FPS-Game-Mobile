using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        volumeSlider.value = GameManager.Instance.VolumeSliderValue;
    }

    public void SetVolume(float volume)
    {
        GameManager.Instance.VolumeSliderValue = volume;
        audioMixer.SetFloat("Volume", Mathf.Log(volume) * 12);
    }

    float GetMixerLevel()
    {
        float volume;
        bool result = audioMixer.GetFloat("Volume", out volume);
        return (result) ? volume : 0f;
    }
}

