using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;


public class CutsceneManager : MonoBehaviour
{
    public static bool IsPlaying { get; private set; } = false;
    public static bool IntroCutscenePlayed { get; set; } = false;

    public PlayableDirector director;
    public DialogueManager dialogueManager;
    public TutorialPanelController tutorialPanelController;

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

    public static void NotifyGameLoaded()
    {
        var manager = FindFirstObjectByType<CutsceneManager>();
        if (manager != null)
            manager.OnGameLoaded();
    }

    public void OnGameLoaded()
    {
        if (!IntroCutscenePlayed)
        {
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
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
        IsPlaying = false;
        ToggleUI(true);
        RefreshAllTextE();
        BlockControls(false);
        Time.timeScale = 1f;
        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded -= ResumeTimeLine;

        IntroCutscenePlayed = true;
        SaveSystem.SaveGame();
        QuestManager.Instance.StartIntroQuest();
        ShowTutorialPanel();
    }

    private void ShowTutorialPanel()
    {
        if (tutorialPanelController != null)
            tutorialPanelController.ShowPanel();
        else
            Debug.LogError("TutorialPanelController not assigned in cutscenemanager");
    }

    private void ToggleUI(bool active)
    {
        foreach (var obj in uiToDisableDuringCutscene)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }

    private void SetAllTextEActive(bool active)
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
        {
            if (npc.TextE != null)
                npc.TextE.SetActive(active);
        }
    }
    private void RefreshAllTextE()
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
        {
            npc.RefreshTextE();
        }
    }

    public void PauseForDialogue()
    {
        Time.timeScale = 0f;
    }

    private void ResumeTimeLine()
    {
        Time.timeScale = 1f;
    }

    private void BlockControls(bool block)
    {
        if (inventoryController != null)
            inventoryController.enabled = !block;
        if (mattock != null)
            mattock.enabled = !block;
        if (wateringCan != null)
            wateringCan.enabled = !block;
        if (Player.Instance != null)
            Player.Instance.SetMovementBlocked(block);
    }
}
