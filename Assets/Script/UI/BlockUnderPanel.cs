using UnityEngine;
using UnityEngine.UI;

public class BlockUnderPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private CanvasGroup canvasGroup;
    private Button button;
    
    void Start()
    {
        canvasGroup = panel.GetComponent<CanvasGroup>();
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
        }
        else
        {
            panel.SetActive(false);
        }
        
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenPanel);
    }
    
    void OpenPanel()
    {
        Debug.Log("Кнопка нажата!");
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true; 
        }
        else
        {
            panel.SetActive(true);
        }
    }
    
    public void ClosePanel()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false; 
        }
        else
        {
            panel.SetActive(false);
        }
    }
}