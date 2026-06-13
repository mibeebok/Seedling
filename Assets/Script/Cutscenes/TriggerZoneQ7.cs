using UnityEngine;

public class TriggerZoneQ7 : MonoBehaviour
{
    public CutsceneQuestSeventhPart1 cutsceneManager;
    private bool cutscenePlayed = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager != null)
                cutsceneManager.StartCutscene();
            else
                Debug.LogError("CutsceneQuestSeventhPart1 не назначен!");
            cutscenePlayed = true;

            QuestManager.Instance.CompleteTask("Найти место, где горел дом");
        }
    }
}
