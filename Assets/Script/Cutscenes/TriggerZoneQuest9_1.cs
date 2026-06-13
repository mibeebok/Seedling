using UnityEngine;

public class TriggerZoneQuest9_1 : MonoBehaviour
{
    public CutsceneQuest9_1 cutsceneManager;
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
                Debug.LogError("CutsceneQuest9_1 не назначен в TriggerZoneQuest9_1");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
