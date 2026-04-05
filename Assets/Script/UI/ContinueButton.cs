using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private ModalWindow modalWindow;
    [SerializeField] private BlockUnderPanel blockUnderPanel;
    
    [Header("Настройки")]
    [SerializeField] private bool hasUnsavedChanges = false;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnCloseClick);

        if (blockUnderPanel == null)
        {
            blockUnderPanel = FindObjectOfType<BlockUnderPanel>();
        }
    }
    
    public void MarkAsChanged()
    {
        hasUnsavedChanges = true;
    }
    
    public void ResetChangesFlag()
    {
        hasUnsavedChanges = false;
    }
    
    void OnCloseClick()
    {
        Debug.Log("OnCloseClick вызван");
        
        if (hasUnsavedChanges && modalWindow != null)
        {
            modalWindow.ShowModal(
                onConfirm: SaveAndClose,
                onDiscard: CloseWithoutSave,
                message: "У вас есть несохраненные изменения. Сохранить?"
            );
        }
        else
        {
            CloseSettings();
        }
    }
    
    void SaveAndClose()
    {
        Debug.Log("Сохранение настроек...ДОДЕЛАТЬ");
        SaveSettings();
        CloseSettings();
    }
    
    void CloseWithoutSave()
    {
        Debug.Log("Закрытие без сохранения");
        CloseSettings();
    }
    
    void CloseSettings()
    {        
        if (settingsPanel != null)
        {
            settingsPanel.alpha = 0;
            settingsPanel.interactable = false;
            settingsPanel.blocksRaycasts = false;
        }
        
        hasUnsavedChanges = false;

        if (blockUnderPanel != null)
        {
            Debug.Log("Вызываем blockUnderPanel.ClosePanel()");
            blockUnderPanel.ClosePanel();
        }
        else
        {            
            CanvasGroup parentMenu = FindObjectOfType<PauseButtonPosition>()?.menu?.GetComponent<CanvasGroup>();
            if (parentMenu != null)
            {
                parentMenu.interactable = true;
                parentMenu.blocksRaycasts = true;
            }
        }
        
        Debug.Log("Панель настроек закрыта");
    }
    
    void SaveSettings()
    {
        PlayerPrefs.Save();
        Debug.Log("Настройки сохранены");
    }
}