using UnityEngine;

public class TriggerZoneQuest13 : MonoBehaviour
{
    public GameObject investigateButton;
    public EvidencePanelUI evidencePanel;
    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            QuestManager.Instance.CompleteTask("Подойти в лавку Терентия");
            if (investigateButton != null)
            {
                investigateButton.SetActive(true);
            }

        }
    }
}
