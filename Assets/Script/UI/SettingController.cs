using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SettingController : MonoBehaviour
{
    public bool isFullScreen = true;
    public AudioMixer am;
    Resolution[] rsl;
    List<string> resolutions;
    public Dropdown dropdown;
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

    public void FulllScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void AudioVolume(float volumeSlider)
    {
        am.SetFloat("masterVolume", volumeSlider);
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
