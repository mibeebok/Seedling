using UnityEngine;
using UnityEngine.UI;
using System;

public class ModalWindow : MonoBehaviour
{
    [Header("Основные элементы")]
    [SerializeField] private CanvasGroup modalCanvasGroup;
    [SerializeField] private Text messageText;
    
    [Header("Кнопки")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button discardButton;
    
    [Header("Настройки")]
    [SerializeField] private string defaultMessage = "Сохранить изменения?";
    
    private Action onConfirmAction;
    private Action onDiscardAction;
    
    void Start()
    {
        HideModal();
        
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClick);
            
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClick);
            
        if (discardButton != null)
            discardButton.onClick.AddListener(OnDiscardClick);
    }
    
    public void ShowModal(Action onConfirm, Action onDiscard, string message = null)
    {
        onConfirmAction = onConfirm;
        onDiscardAction = onDiscard;
        
        if (messageText != null)
            messageText.text = message ?? defaultMessage;
        
        if (modalCanvasGroup != null)
        {
            modalCanvasGroup.alpha = 1;
            modalCanvasGroup.interactable = true;
            modalCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    
    public void HideModal()
    {
        if (modalCanvasGroup != null)
        {
            modalCanvasGroup.alpha = 0;
            modalCanvasGroup.interactable = false;
            modalCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        onConfirmAction = null;
        onDiscardAction = null;
    }
    
    void OnConfirmClick()
    {
        Debug.Log("Подтверждено - сохранить изменения ДОДЕЛАТЬ");
        onConfirmAction?.Invoke();
        HideModal();
    }
    
    void OnDiscardClick()
    {
        Debug.Log("Не сохранять изменения");
        onDiscardAction?.Invoke();
        HideModal();
    }
    
    void OnCancelClick()
    {
        Debug.Log("Отмена закрытия");
        HideModal();
    }
    
    // Обновление текста (можно вызывать динамически)
    public void SetMessage(string newMessage)
    {
        if (messageText != null)
            messageText.text = newMessage;
    }
}