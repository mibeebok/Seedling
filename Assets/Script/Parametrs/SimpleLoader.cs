using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SinmpleLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float minLoadTime = 2f;
    [SerializeField] private GameObject[] objectsToWaitFor; // Объекты с RandomGeneration

    void Start()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(WaitForGameReady());
    }

    IEnumerator WaitForGameReady()
    {
        float timer = 0f;

        yield return null;

        while (timer < minLoadTime || !AllGenerationComplete())
        {
            timer += Time.deltaTime;

            float progress = Mathf.Min(timer / minLoadTime, 0.9f);
            if (AllGenerationComplete() && timer >= minLoadTime)
                progress = 1f;

            loadingSlider.value = progress;
            yield return null;
        }

        loadingSlider.value = 1f;
        yield return new WaitForSeconds(0.3f);
        loadingPanel.SetActive(false);
    }

    bool AllGenerationComplete()
    {
        if (objectsToWaitFor == null || objectsToWaitFor.Length == 0)
            return true;

        foreach (var obj in objectsToWaitFor)
        {
            if (obj == null) continue;

            var gen = obj.GetComponent<RandomGeneration>();
            if (gen != null && !gen.IsGenerationComplete)
                return false;
        }
        return true;
    }
}