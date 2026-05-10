using UnityEngine;
using UnityEngine.UI;

public class CropInfoUI : MonoBehaviour
{
    public static CropInfoUI Instance { get; private set; }
    public GameObject infoPanel;
    public Text infoText;
    
    private void Awake()
    {
        Instance = this;
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void ShowInfo(Vector3 worldPosition, string text)
    {
        if (infoPanel == null) return;
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        infoPanel.transform.position = screenPos;
        infoText.text = text;
        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}