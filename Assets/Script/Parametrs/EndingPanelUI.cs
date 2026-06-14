using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingPanelUI : MonoBehaviour
{
    public Text endingText;
    public Button exitButton;
    public string startSceneName = "StartScene";

    void Start()
    {
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToMainMenu);
    }

    private void ExitToMainMenu()
    {
        SceneManager.LoadScene(startSceneName);
    }
}
