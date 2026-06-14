using UnityEngine;

public class TriggerZoneQuest11 : MonoBehaviour
{
    public CutsceneQuest11 cutsceneManager;
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
                Debug.LogError("CutsceneQuest11 не назначен в TriggerZoneQuest11");

            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
}
