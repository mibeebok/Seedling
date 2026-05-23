using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SettingController : MonoBehaviour
{
    [Tooltip("Текущее состояние полноэкранного режима")]
    public bool isFullScreen = true;

    [Header("Аудиомикшер")]
    [Tooltip("Основной AudioMixer для SFX. В сцене меню оставьте поле пустым")]
    public AudioMixer audioMixer;

    [Header("Элементы интерфейса")]
    [Tooltip("Выпадающий список разрешений экрана")]
    public Dropdown dropdown;
    [Tooltip("Слайдер громкости музыки")]
    public Slider musicSlider;
    [Tooltip("Слайдер громкости звуковых эффектов (SFX)")]
    public Slider sfxSlider;
    [Tooltip("Панель настроек (будет скрываться при закрытии)")]
    public GameObject settingsPanel;
    [Tooltip("Блокировка под панелью (если используется). Если нет – панель просто скроется")]
    public BlockUnderPanel blockUnderPanel;

    [Header("Источники музыки")]
    [Tooltip("AudioSource фоновой музыки в главном меню (если есть). В игре можно оставить пустым")]
    public AudioSource menuMusicSource;

    Resolution[] rsl;
    List<string> resolutions;

    public void Awake()
    {
        resolutions = new List<string>();
        rsl = Screen.resolutions;
        foreach (var i in rsl)
        {
            resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(resolutions);
    }

    void Start()
    {
        //музыка
        if (musicSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            musicSlider.value = savedVolume;
            ApplyMusicVolume(savedVolume);
            musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        }

        // SFX
        if (sfxSlider != null)
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            sfxSlider.value = savedSFXVolume;
            ApplySFXVolume(savedSFXVolume);
            sfxSlider.onValueChanged.AddListener(ApplySFXVolume);
        }

        //resolution
        if (dropdown != null && rsl != null && rsl.Length > 0)
        {
            int maxResIndex = 0;
            int maxArea = 0;
            for (int i=0; i<rsl.Length; i++)
            {
                int area = rsl[i].width * rsl[i].height;
                if (area > maxArea)
                {
                    maxArea = area;
                    maxResIndex =i;
                }
            }
            dropdown.value = maxResIndex;
            Resolution(maxResIndex);
        }
    }

    public void ApplyMusicVolume(float volume)
    {
        if (menuMusicSource != null)
            menuMusicSource.volume = volume;

        FarmGrid farmGrid = FindObjectOfType<FarmGrid>();
        if (farmGrid != null)
        {
            AudioSource musicSource = farmGrid.GetComponent<AudioSource>();
            if (musicSource != null)
                musicSource.volume = volume;
        }

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void ApplySFXVolume(float volume)
    {
        if (audioMixer != null)
        {
            float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("SFXVolume", dB);
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void CloseSettings()
    {
        if (blockUnderPanel != null)
            blockUnderPanel.ClosePanel();
        else if (settingsPanel != null)
            settingsPanel.SetActive(false);
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

    public void OpenSetting()
    {
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.3f);

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveListener(ApplyMusicVolume);
            musicSlider.value = savedMusic;
            musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        } 
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }
}