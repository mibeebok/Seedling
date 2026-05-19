using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private string sceneToLoad = "SampleScene";

    private bool isLoading = false;

    public void StartLoading()
    {
        if (isLoading) return;
        isLoading = true;

        loadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            loadingSlider.value = operation.progress / 0.9f;
            yield return null;
        }

        loadingSlider.value = 1f;

        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }
}