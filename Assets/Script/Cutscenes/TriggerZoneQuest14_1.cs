using UnityEngine;

public class TriggerZoneQuest14_1 : MonoBehaviour
{
    public CutsceneQuest14_1 cutsceneManager;
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
                Debug.LogError("CutsceneQuest14_1 не назначен в TriggerZoneQuest14_1");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
