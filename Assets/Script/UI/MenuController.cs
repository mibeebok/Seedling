using UnityEngine;

using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Кнопки меню")]
    public GameObject buttonContinue;
    public GameObject buttonSave;
    public GameObject buttonExit;
    public GameObject buttonSetting;

    [Header("Ссылки")]
    public GameObject menuPanel;
    public PauseButtonPosition pauseButtonController;

    public bool isOpened = false;

    void Start()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Не назначен объект меню (menuPanel)!");
        }

        if (pauseButtonController == null)
        {
            //pauseButtonController = FindObjectOfType<PauseButtonPosition>();
            if (pauseButtonController == null)
            {
                Debug.LogError("PauseButtonPosition не найден на сцене!");
            }
            else
            {
                Debug.Log("PauseButtonPosition найден автоматически");
            }
        }

        if (buttonContinue != null)
        {
            UnityEngine.UI.Button btn = buttonContinue.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(ContinueButton);
            }
            else
            {
                Debug.LogError("Компонент Button не найден на buttonContinue!");
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowHideMenu();
        }
    }

    public void ContinueButton()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Debug.Log("Меню закрыто");
        }

        if (pauseButtonController != null)
        {
            pauseButtonController.ResumeGame();
        }
        else
        {
            Debug.LogWarning("Контроллер паузы не найден, использую Time.timeScale");
            Time.timeScale = 1f;
        }
    }

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        GetComponent<Canvas> ().enabled = isOpened;//Выключение или отключение canvas
    }

}