using UnityEngine;
using UnityEngine.UI;

public class OpenSetting : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private Button b1;

    void Start()
    {
        b1 = GetComponent<Button>();
        b1.onClick.AddListener(OpenSettingOnClick);
    }
    void OpenSettingOnClick()
    {
        panel.SetActive(true);
    }
}
