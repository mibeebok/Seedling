using UnityEngine;

public class FinnickTriggerZoneQ3 : MonoBehaviour
{
    public CutsceneQuestThird cutsceneManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneQuestThird не назначен в FinnickTriggerZoneQ3");
            gameObject.SetActive(false);
        }
    }
}
