using UnityEngine;

public class SettingController : MonoBehaviour
{
    private bool isFullScreen = false;
    public void FulllScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }
}
