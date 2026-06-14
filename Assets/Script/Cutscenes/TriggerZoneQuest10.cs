using UnityEngine;

public class TriggerZoneQuest10 : MonoBehaviour
{
    public CutsceneQuest10 cutsceneManager;
    private Collider2D triggerCollider;
    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneQuestTenth не назначен в TriggerZoneQuest10");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
