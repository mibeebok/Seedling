using UnityEngine;

public class HomeTriggerForCutscenes : MonoBehaviour
{
    public CutsceneManagerHouse cutsceneManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneManagerHouse не назначен");
            gameObject.SetActive(false);
        }
    }
}
