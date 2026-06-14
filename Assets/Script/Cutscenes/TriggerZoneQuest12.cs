using UnityEngine;

public class TriggerZoneQuest12 : MonoBehaviour
{
    public CutsceneQuest12 cutsceneManager;
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
                Debug.LogError("CutsceneQuest12 не назначен в TriggerZoneQuest12");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
