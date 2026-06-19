using UnityEngine;
using UnityEngine.UI;

public class UiTooltip : MonoBehaviour
{
    public static UiTooltip Instance { get; private set; }
    
    public GameObject tooltipPanel;
    public Text tooltipText;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }
    
    public void Show(string itemName)
    {
        if (tooltipPanel == null || tooltipText == null) return;
        
        tooltipText.text = itemName;
        tooltipPanel.SetActive(true);
        
        // Позиционируем за мышкой
        Vector2 mousePos = Input.mousePosition;
        tooltipPanel.transform.position = mousePos + new Vector2(100, -20);
    }
    
    public void Hide()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }
}