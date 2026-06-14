using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EvidencePanelUI : MonoBehaviour
{
    public GameObject panel;
    public Text clueCounterText;
    public Button closeButton;
    public GameObject[] furButtons; 
    public GameObject cutsceneStarter;
    public GameObject investigateButton;

    private int collectedCount = 0;
    private bool allCollected = false;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
        UpdateCounter();

        for (int i = 0; i < furButtons.Length; i++)
        {
            int idx = i;
            Button btn = furButtons[i].GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => OnFurClick(idx));
        }
    }

    private void OnFurClick(int index)
    {
        if (allCollected) return;
        if (furButtons[index].activeSelf)
        {
            furButtons[index].SetActive(false);
            collectedCount++;
            UpdateCounter();

            if (collectedCount >= furButtons.Length)
            {
                allCollected = true;
                QuestManager.Instance.CompleteTask("Начать детальный осмотр улики");
            }
        }
    }

    private void UpdateCounter()
    {
        if (clueCounterText != null)
            clueCounterText.text = $"Улика: {collectedCount}/{furButtons.Length}";
    }

    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
        if (investigateButton != null)
            investigateButton.SetActive(false);
    }

    private void ClosePanel()
    {
        if (panel != null) panel.SetActive(false);

        if (allCollected)
        {
            if (cutsceneStarter != null)
                cutsceneStarter.SetActive(true);
            if (investigateButton != null)
                investigateButton.SetActive(false);
        }
        else
        {
            if (investigateButton != null)
                investigateButton.SetActive(true);
        }
    }
}
