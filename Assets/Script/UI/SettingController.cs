using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingController : MonoBehaviour
{
    public bool isFullScreen = true;
    
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [Header("UI")]
    public Dropdown dropdown;
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject settingsPanel;
    public BlockUnderPanel blockUnderPanel; // Добавьте ссылку
    
    Resolution[] rsl;
    List<string> resolutions;
    
    public void Awake() 
    {
        resolutions = new List<string>();
        rsl = Screen.resolutions;
        foreach(var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);
    }
    
    void Start()
    {
        if (musicSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            musicSlider.value = savedVolume;
            ApplyMusicVolume(savedVolume);
            musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        }
        
        if (sfxSlider != null)
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            sfxSlider.value = savedSFXVolume;
            ApplySFXVolume(savedSFXVolume);
            sfxSlider.onValueChanged.AddListener(ApplySFXVolume);
        }
    }
    
    public void ApplyMusicVolume(float volume)
    {
        FarmGrid farmGrid = FindObjectOfType<FarmGrid>();
        
        if (farmGrid != null)
        {
            AudioSource musicSource = farmGrid.GetComponent<AudioSource>();
            if (musicSource != null)
            {
                musicSource.volume = volume;
            }
        }
        
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
    
    public void ApplySFXVolume(float volume)
    {
        float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void CloseSettings()
    {
        // Используем BlockUnderPanel если он есть
        if (blockUnderPanel != null)
        {
            blockUnderPanel.ClosePanel();
        }
        // Или просто деактивируем панель
        else if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void FulllScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void AudioVolume(float volumeSlider)
    {
        ApplyMusicVolume(volumeSlider);
    }
    
    public void Quality(int q)
    {
        QualitySettings.SetQualityLevel(q);
    }
    
    public void Resolution(int r)
    {
        Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
    }
}