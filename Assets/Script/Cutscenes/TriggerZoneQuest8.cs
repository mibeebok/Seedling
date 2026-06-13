using UnityEngine;

public class TriggerZoneQuest8 : MonoBehaviour
{
    public CutsceneQuest8 cutsceneManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneQuestEighth не назначен в TriggerZoneQuest8");
            gameObject.SetActive(false);
        }
    }
}
