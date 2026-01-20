using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private ModalWindow modalWindow;
    
    [Header("Настройки")]
    [SerializeField] private bool hasUnsavedChanges = false;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnCloseClick);
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
        
        // Здесь код сохранения настроек
        // Например:
        // PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        // PlayerPrefs.Save();
        
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
        
        Debug.Log("Панель настроек закрыта");
    }
    
    void SaveSettings()
    {
        PlayerPrefs.Save();
        Debug.Log("Настройки сохранены ДОДЕЛАТЬ ЭТО ПРИМЕР");
    }
}