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
    public Dropdown dropdown;
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject settingsPanel;
    public BlockUnderPanel blockUnderPanel;

    [Header("Источники музыки")]
    public AudioSource menuMusicSource;

    [Header("Бэкап")]
    public GameObject panelBackup;
    public GameObject buttonLoadBackup;

    Resolution[] rsl;
    List<string> resolutions;

    // Флаг для безопасной установки слайдера (если SetValueWithoutNotify недоступен)
    // private bool ignoreSliderChange = false;

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
        // ------------------ МУЗЫКА ------------------
        if (musicSlider != null)
        {
            // Определяем текущий источник музыки (какой есть на сцене)
            AudioSource currentMusic = menuMusicSource;
            if (currentMusic == null)
            {
                FarmGrid farmGrid = FindObjectOfType<FarmGrid>();
                if (farmGrid != null)
                    currentMusic = farmGrid.GetComponent<AudioSource>();
            }

            // Реальная громкость источника (если источника нет, берём 1)
            float currentVolume = currentMusic != null ? currentMusic.volume : 1f;

            // Устанавливаем слайдер без вызова события
            #if UNITY_2019_4_OR_NEWER
                musicSlider.SetValueWithoutNotify(currentVolume);
            #else
                // Ручная блокировка события для старых версий Unity
                musicSlider.onValueChanged.RemoveAllListeners();
                musicSlider.value = currentVolume;
            #endif

            // Добавляем обработчик изменения слайдера
            musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        }

        // ------------------ SFX ------------------
        if (sfxSlider != null)
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            #if UNITY_2019_4_OR_NEWER
                sfxSlider.SetValueWithoutNotify(savedSFXVolume);
            #else
                sfxSlider.onValueChanged.RemoveAllListeners();
                sfxSlider.value = savedSFXVolume;
            #endif
            // Применяем SFX громкость из сохранения (логично для микшера)
            ApplySFXVolume(savedSFXVolume);
            sfxSlider.onValueChanged.AddListener(ApplySFXVolume);
        }

        // ------------------ РАЗРЕШЕНИЕ ------------------
        if (dropdown != null && rsl != null && rsl.Length > 0)
        {
            int maxResIndex = 0;
            int maxArea = 0;
            for (int i = 0; i < rsl.Length; i++)
            {
                int area = rsl[i].width * rsl[i].height;
                if (area > maxArea)
                {
                    maxArea = area;
                    maxResIndex = i;
                }
            }
            dropdown.value = maxResIndex;
            Resolution(maxResIndex);
        }

        if (buttonLoadBackup != null)
        {
            Button btn = buttonLoadBackup.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(LoadBackupButton);
        }
    }

    /// <summary>Применяет громкость музыки к активному источнику.</summary>
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

        // Сохраняем для восстановления при следующем запуске (если нужно)
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

    /// <summary>Открывает панель настроек и синхронизирует слайдер музыки с актуальной громкостью.</summary>
    public void OpenSetting()
    {
        if (musicSlider != null)
        {
            // Снова определяем текущий источник музыки
            AudioSource currentMusic = menuMusicSource;
            if (currentMusic == null)
            {
                FarmGrid farmGrid = FindObjectOfType<FarmGrid>();
                if (farmGrid != null)
                    currentMusic = farmGrid.GetComponent<AudioSource>();
            }

            float currentVolume = currentMusic != null ? currentMusic.volume : 1f;

            // Обновляем слайдер без вызова обработчиков
            #if UNITY_2019_4_OR_NEWER
                musicSlider.SetValueWithoutNotify(currentVolume);
            #else
                musicSlider.onValueChanged.RemoveAllListeners();
                musicSlider.value = currentVolume;
                musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
            #endif
        }

        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // Остальные методы без изменений
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

    public void LoadBackupButton()
    {
        SaveSystem.LoadBackupGame();
        if (panelBackup != null)
        {
            panelBackup.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }
}