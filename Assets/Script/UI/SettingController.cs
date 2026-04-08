using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingController : MonoBehaviour
{
    public bool isFullScreen = true;
    
    [Header("UI")]
    public Dropdown dropdown;
    public Slider musicSlider;
    
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
        // Настройка слайдера громкости
        if (musicSlider != null)
        {
            // Загружаем сохранённое значение
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            musicSlider.value = savedVolume;
            
            // Применяем громкость сразу
            ApplyMusicVolume(savedVolume);
            
            // Подписываемся на изменения
            musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        }
    }
    
    private void ApplyMusicVolume(float volume)
    {
        FarmGrid farmGrid = FindObjectOfType<FarmGrid>();
        
        if (farmGrid != null)
        {
            AudioSource musicSource = farmGrid.GetComponent<AudioSource>();
            if (musicSource != null)
            {
                musicSource.volume = volume;
            }
            
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
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