using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] _resolutions;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public AudioMixer mainAudioMixer;

    
    private void Start()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", -40);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", -40);
        sfxVolume.value = PlayerPrefs.GetFloat("SfxVolume", -40);
        
        fullScreenToggle.isOn = Screen.fullScreen;
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<String> options = new List<string>();

        int currentIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width
                && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();
        ChangeMasterVolume();
        ChangeMusicVolume();
        ChangeSfxVolume();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
    }
    
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
    }
    
    public void ChangeSfxVolume()
    {
        mainAudioMixer.SetFloat("SfxVolume", sfxVolume.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume.value);
    }
}
