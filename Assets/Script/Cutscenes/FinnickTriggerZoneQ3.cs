using UnityEngine;

public class FinnickTriggerZoneQ3 : MonoBehaviour
{
    public CutsceneQuestThird cutsceneManager;
    public GameObject arrowIndicator;
    private void OnEnable()
    {
        if (arrowIndicator != null)
            arrowIndicator.SetActive(true);
    }

    private void OnDisable()
    {
        if (arrowIndicator != null)
            arrowIndicator.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
            {
                if (cutsceneManager != null && !CutsceneQuestThird.hasPlayed)
                    cutsceneManager.StartCutscene();
            }
            else
                Debug.LogError("CutsceneQuestThird не назначен в FinnickTriggerZoneQ3");
            gameObject.SetActive(false);
        }
    }
}
