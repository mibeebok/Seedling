using UnityEngine;

public class TriggerZoneQuest15 : MonoBehaviour
{
    public CutsceneQuest15 cutsceneManager;
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
                Debug.LogError("CutsceneQuest15 не назначен в TriggerZoneQuest15");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
