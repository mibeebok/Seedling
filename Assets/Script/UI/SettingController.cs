using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingController : MonoBehaviour
{
    public float volume = 0;//громкость
    public bool IsSound = true;//звуки
    public bool IsMusic = true;//музыка
    public int quality = 0;//качество
    public bool isFullscreen = false;//полноэкранный режим
    public AudioMixer audioMixer;//Регулятор громкости
    public Dropdown resolutionDropdown;//список с разрешениями для игры
    private Resolution[] resolutions;//Список доступных разрешений
    private int currResolutionIndex =0;//Текущее разрешение
    public Dropdown qualityDropdown;//список с качеством изображения
    public bool isOpened = false;
    
    void Update()
    {
        ShowHideMenu();
    }
    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        GetComponent<Canvas>().enabled = isOpened;
    }
    public void ChangeVolume (float val)//изменение громкости
    {
        volume = val;
    }
    public void ChangeSound (bool val)//включить отключить звуки
    {
        IsSound = val;
    }
    public void ChangeMusic (bool val)//включить отвключить музыку
    {
        IsMusic = val;
    }
    public void ChangeQuality (int index)//изменение качества
    {
        quality = index;
    }
    public void ChangeFullscreenMode (bool val)//включение отключение полноэкранного режима
    {
        isFullscreen = val;
    }
    public void ChangeResolution (int index)//изменнеие разрешения
    {
        currResolutionIndex = index;
    }
    public void SaveSettings()
    {
        audioMixer.SetFloat("MasterVolume", volume);//изменение уровня громкости
        QualitySettings.SetQualityLevel(quality);//изменение качества
        Screen.fullScreen = isFullscreen;//вкл откл полноэкранного режима
        Screen.SetResolution(Screen.resolutions[currResolutionIndex].width, Screen.resolutions[currResolutionIndex].height, isFullscreen);//изменение разрешения
    }
}
