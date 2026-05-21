using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Button))]
public class ExitController : MonoBehaviour
{
    [Header("Основные настройки")]
    [Tooltip("Окно подтверждения выхода")]
    [SerializeField] private GameObject exitWindow;
    
    [Tooltip("Кнопка подтверждения выхода")]
    [SerializeField] private Button confirmButton;
    
    [Tooltip("Кнопка отмены выхода")]
    [SerializeField] private Button cancelButton;
    
    [Tooltip("Задержка между кликами (секунды)")]
    [SerializeField] private float clickCooldown = 0.5f;
    [Header("Действие при подтверждении")]
    [Tooltip("Если включено - загружает сцену Game вместо выхода из приложения")]
    [SerializeField] private bool returnToMainMenu = false;
    [SerializeField]private string mainSceneName = "Game";

    private Button _exitButton;
    private float _lastClickTime;
    private Canvas _parentCanvas;

    private void Awake()
    {
        _exitButton = GetComponent<Button>();
        
    }

    private void Start()
    {
        _parentCanvas = exitWindow.GetComponentInParent<Canvas>();
        
        if (exitWindow != null)
        {
            exitWindow.SetActive(false);
            CenterWindow();
        }

        _exitButton.onClick.AddListener(OnExitButtonClick);
        
        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirmClick);
        
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelClick);
    }

    private void OnExitButtonClick()
    {
        if (Time.unscaledTime - _lastClickTime < clickCooldown) return;
        _lastClickTime = Time.unscaledTime;
        
        ToggleExitWindow();
    }

    private void ToggleExitWindow()
    {
        if (exitWindow == null) return;
        
        bool shouldOpen = !exitWindow.activeSelf;
        exitWindow.SetActive(shouldOpen);
        
        if (shouldOpen)
        {
            CenterWindow();
        }
    }

    private void CenterWindow()
    {
        if (exitWindow == null) return;

        if (exitWindow.TryGetComponent<RectTransform>(out var rectTransform))
        {
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            exitWindow.transform.position = GetScreenCenter();
        }
    }

    private Vector3 GetScreenCenter()
    {
        if (Camera.main != null)
            return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
        
        return new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
    }

    private void OnConfirmClick()
    {
        if (returnToMainMenu)
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    private void OnCancelClick()
    {
        ToggleExitWindow();
    }

    private void OnDestroy()
    {
        if (_exitButton != null)
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        
        if (confirmButton != null)
            confirmButton.onClick.RemoveListener(OnConfirmClick);
        
        if (cancelButton != null)
            cancelButton.onClick.RemoveListener(OnCancelClick);
    }
}