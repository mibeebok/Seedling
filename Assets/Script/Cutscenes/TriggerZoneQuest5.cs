using UnityEngine;

public class TriggerZoneQuest5 : MonoBehaviour
{
    public CutsceneQuestFifth cutsceneManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneQuestFifth не назначен в IhvilnichtTriggerZoneQ5");
            gameObject.SetActive(false);
        }
    }
}
