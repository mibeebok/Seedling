using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
