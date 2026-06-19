using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceTooltip : MonoBehaviour
{
    public static WorldSpaceTooltip Instance { get; private set; }
    
    public GameObject tooltipPanel;      // Панель с Image
    public Text tooltipText;             // Текст внутри панели
    
    [Header("Settings")]
    public Vector2 offset = new Vector2(50, -30); // Отступ от мыши в пикселях
    public float showDelay = 0.3f;
    
    private Camera mainCamera;
    private string currentText;
    private float hoverTimer;
    private bool shouldShow;
    private RectTransform panelRect;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        mainCamera = Camera.main;
        
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
            panelRect = tooltipPanel.GetComponent<RectTransform>();
        }
    }
    
    private void Update()
    {
        if (shouldShow)
        {
            hoverTimer += Time.deltaTime;
            
            if (hoverTimer >= showDelay)
            {
                ShowTooltipPanel();
                UpdatePosition();
            }
        }
        else
        {
            HideTooltipPanel();
            hoverTimer = 0f;
        }
    }
    
    public void Show(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Hide();
            return;
        }
        
        currentText = itemName;
        shouldShow = true;
        hoverTimer = 0f;
    }
    
    public void Hide()
    {
        shouldShow = false;
        hoverTimer = 0f;
        currentText = "";
        HideTooltipPanel();
    }
    
    private void ShowTooltipPanel()
    {
        if (tooltipPanel == null || tooltipText == null) return;
        
        tooltipText.text = currentText;
        
        if (!tooltipPanel.activeSelf)
            tooltipPanel.SetActive(true);
    }
    
    private void HideTooltipPanel()
    {
        if (tooltipPanel != null && tooltipPanel.activeSelf)
            tooltipPanel.SetActive(false);
    }
    
    private void UpdatePosition()
    {
        if (mainCamera == null || panelRect == null) return;
        
        // Получаем позицию мыши в экранных координатах
        Vector3 mouseScreenPos = Input.mousePosition;
        
        // Добавляем отступ
        Vector3 finalPos = mouseScreenPos + new Vector3(offset.x, offset.y, 0);
        
        // Корректируем, чтобы не выходило за пределы экрана
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;
        
        if (finalPos.x + panelWidth > Screen.width)
            finalPos.x = mouseScreenPos.x - panelWidth - offset.x;
        
        if (finalPos.y - panelHeight < 0)
            finalPos.y = mouseScreenPos.y + panelHeight - offset.y;
        
        panelRect.position = finalPos;
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}