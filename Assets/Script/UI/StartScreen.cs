using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void PlayPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("ExitPressed");
    }
}
