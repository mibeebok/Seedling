using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class BlockUnderPanel : MonoBehaviour
{
    [Header("Настрйоки")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject parentMenu;
    private CanvasGroup panelCanvasGroup;
    private CanvasGroup parentMenuCanvasGroup;
    private Button button;
    private bool isPanelOpen = false;

    void Start()
    {
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = panel.AddComponent<CanvasGroup>();

        }

        if (parentMenu != null)
        {
            parentMenuCanvasGroup = parentMenu.GetComponent<CanvasGroup>();
            if(parentMenuCanvasGroup == null)
            {
                parentMenuCanvasGroup = parentMenu.AddComponent<CanvasGroup>();
            }
        }

        panelCanvasGroup.alpha =0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        button = GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(TogglePanel);
        }
    }

    public void TogglePanel()
    {
        if (isPanelOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }
    public void OpenPanel()
    {
        panelCanvasGroup.alpha =1;
        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;

        if (parentMenuCanvasGroup != null)
        {
            parentMenuCanvasGroup.interactable = false;
            parentMenuCanvasGroup.blocksRaycasts = false;
        }
        isPanelOpen = true;
    }

    public void ClosePanel()
    {
        panelCanvasGroup.alpha =0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        if(parentMenuCanvasGroup != null)
        {
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.blocksRaycasts = true;
        }
        isPanelOpen=false;
    }
    public bool IsPanelOpen()
    {
        return isPanelOpen;
    }
 
}