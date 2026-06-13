using UnityEngine;
using UnityEngine.Playables;

public class CutsceneQuestSeventhPart2 : MonoBehaviour
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
        inventoryController = FindObjectOfType<InventoryController>();
        mattock = FindObjectOfType<MattockController>();
        wateringCan = FindObjectOfType<WateringCanController>();

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

        GameObject tiolli = GameObject.Find("rabbit_0");
        if (tiolli != null)
            tiolli.transform.position = new Vector3(6.91f, 2.6f, 0f);

        QuestManager.Instance.CompleteTask("Осмотреться на предмет улик");
    }
    private void ToggleUI(bool active)
    {
        foreach (var obj in uiToDisableDuringCutscene)
            if (obj != null) obj.SetActive(active);
    }

    private void SetAllTextEActive(bool active)
    {
        var allNPCs = FindObjectsOfType<NPCInteraction>();
        foreach (var npc in allNPCs)
            if (npc.TextE != null) npc.TextE.SetActive(active);
    }

    private void RefreshAllTextE()
    {
        var allNPCs = FindObjectsOfType<NPCInteraction>();
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
        dialogueManager.StartDialogueByKey("DialogueQuest7.2", "Тиоли");
    }

}
