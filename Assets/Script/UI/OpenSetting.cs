using UnityEngine;
using UnityEngine.UI;

public class OpenSetting : MonoBehaviour
{
    [SerializeField] private BlockUnderPanel blockUnderPanel;

    private Button b1;

    void Start()
    {
        b1 = GetComponent<Button>();
        b1.onClick.AddListener(OpenSettingOnClick);
    }

    void OpenSettingOnClick()
    {
        blockUnderPanel.OpenPanel();
    }
    void CloseCurrentPanel()
    {
        blockUnderPanel.ClosePanel();
    }
}
