using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManagerHouse : MonoBehaviour
{
    public static bool IsPlaying { get; private set; } = false;

    public PlayableDirector director;
    public DialogueManager dialogueManager;

    public GameObject[] uiToDisableDuringCutscene;

    private InventoryController inventoryController;
    private MattockController mattock;
    private WateringCanController wateringCan;

    void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        mattock = FindFirstObjectByType<MattockController>();
        wateringCan = FindFirstObjectByType<WateringCanController>();

        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded += ResumeTimeLine;

        if (director != null)
        {
            director.played += OnCutsceneStart;
            director.stopped += OnCutsceneEnd;
        }
    }
    public void StartCutscene()
    {
        GameState.IsCutscenePlaying = true;
        SetAllTextEActive(false);
        ToggleUI(false);
        BlockControls(true);
        IsPlaying = true;
        if (director != null)
            director.Play();

        var tutorialHome = FindFirstObjectByType<TutorialHomeController>();
        if (tutorialHome != null) tutorialHome.HidePanel();
    }
    void OnCutsceneStart(PlayableDirector pd)
    {
        SetAllTextEActive(false);
        ToggleUI(false);
        BlockControls(true);
        IsPlaying = true;
    }

    void OnCutsceneEnd(PlayableDirector pd)
    {
        GameState.IsCutscenePlaying = false;
        IsPlaying = false;
        ToggleUI(true);
        RefreshAllTextE();
        BlockControls(false);
        Time.timeScale = 1f;
        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded -= ResumeTimeLine;

        QuestManager.Instance.CompleteTask("Найти свой дом");
    }
    private void ToggleUI(bool active)
    {
        foreach (var obj in uiToDisableDuringCutscene)
            if (obj != null) obj.SetActive(active);
    }

    private void SetAllTextEActive(bool active)
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
            if (npc.TextE != null) npc.TextE.SetActive(active);
    }
    private void RefreshAllTextE()
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
            npc.RefreshTextE();
    }

    public void PauseForDialogue()
    {
        Time.timeScale = 0f;
    }
    private void ResumeTimeLine(string npcName)
    {
        Time.timeScale = 1f;
    }

    private void BlockControls(bool block)
    {
        if (inventoryController != null) inventoryController.enabled = !block;
        if (mattock != null) mattock.enabled = !block;
        if (wateringCan != null) wateringCan.enabled = !block;
        if (Player.Instance != null) Player.Instance.SetMovementBlocked(block);
    }
    public void PauseAndStartDialogue()
    {
        PauseForDialogue();
        dialogueManager.StartDialogueByKey("TioliDialogueQuest2.2", "Тиоли");
    }

}
