using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Burst.CompilerServices;
public class DialogueManager : MonoBehaviour
{
    public System.Action<string> OnDialogueEnded;
    public System.Action<string, int, int> OnChoiceSelectedEvent;

    [Header("UI Elements")]
    public GameObject dialogueBox;
    public Image dialogueBackgroundImage;
    public Image faceImage;
    public Text nameText;
    public Text dialogueText;
    public GameObject hintText;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    [Header("Sprites")]
    public Sprite npcDialogueSprite;
    public Sprite playerDialogueSprite;
    public Sprite playerFace;
    public string playerName;

    [Header("NPC Sprites")]
    public Sprite tioliSprite;
    public Sprite ihvilnichtSprite;
    public Sprite finnickSprite;
    public Sprite terentySprite;

    private InventoryController inventoryController;
    private MattockController mattock;
    private WateringCanController wateringCan;

    public ShopUI shopUI;

    private List<DialogueLine> lines;
    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private Coroutine glowCoroutine;
    public RectTransform hintRect;

    private bool isChoiceActive = false;
    private bool isShopOpened = false;

    private string currentNPCName;
    private Dictionary<string, Sprite> npcSprites;

    private string currentDialogueKey;
    public TutorialHomeController tutorialHomeController;
    public GameObject houseTriggerZone;

    private void Awake()
    {
        npcSprites = new Dictionary<string, Sprite>();
    }

    private void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();
        mattock = FindFirstObjectByType<MattockController>();
        wateringCan = FindFirstObjectByType<WateringCanController>();

        npcSprites["Тиоли"] = tioliSprite;
        npcSprites["Ихвильнихт"] = ihvilnichtSprite;
        npcSprites["Финник"] = finnickSprite;
        npcSprites["Терентий"] = terentySprite;

        if (hintText != null) hintText.SetActive(false);
        if (choiceContainer != null) choiceContainer.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!isDialogueActive) return;

        if (isChoiceActive)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[currentIndex].text;
                isTyping = false;
                if (hintText != null) StartGlowHint();
            }
            else
            {
                ShowNextLine();
            }
        }

    }

    public void StartDialogueByKey(string dialogueKey)
    {
        currentDialogueKey = dialogueKey;
        List<DialogueLine> dialogue = DialogueDatabase.GetDialogue(dialogueKey);
        if (dialogue == null || dialogue.Count == 0)
        {
            Debug.LogError($"Диалог с ключом '{dialogueKey}' не найден!");
            return;
        }

        string firstSpeaker = dialogue[0].speakerName;
        Sprite firstFace = GetSpriteForSpeaker(firstSpeaker);
        StartDialogue(dialogue, firstSpeaker, firstFace);
    }

    public void StartDialogueByKey(string dialogueKey, string npcName)
    {
        currentDialogueKey = dialogueKey;
        List<DialogueLine> dialogue = DialogueDatabase.GetDialogue(dialogueKey);
        if (dialogue == null || dialogue.Count == 0)
        {
            Debug.LogError($"Dialogue with key '{dialogueKey}' not found!");
            return;
        } 
        Sprite npcFace = GetSpriteForSpeaker(npcName);
        StartDialogue(dialogue, npcName, npcFace);
    }

    public void StartIntroDialogue() => StartDialogueByKey("Intro");

    private void StartDialogue(List<DialogueLine> dialogueLines, string npcName, Sprite npcFace)
    {
        lines = dialogueLines;
        currentIndex = 0;
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        currentNPCName = npcName;

        if (Player.Instance != null)
            Player.Instance.SetMovementBlocked(true);
        if (inventoryController != null)
            inventoryController.enabled = false;
        if (mattock != null)
            mattock.enabled = false;
        if (wateringCan != null)
            wateringCan.enabled = false;

        ResetDialogueToNPC(npcFace, npcName);
        dialogueText.gameObject.SetActive(true);
        if (choiceContainer != null) choiceContainer.gameObject.SetActive(false);
        if (hintText != null) hintText.SetActive(false);

        ShowCurrentLine();
    }

    public void StartDialogueFromCode(List<DialogueLine> dialogueLines, string speakerName)
    {
        Sprite speakerFace = GetSpriteForSpeaker(speakerName);
        StartDialogue(dialogueLines, speakerName, speakerFace);
    }

    private void ResetDialogueToNPC(Sprite npcFace, string npcName)
    {
        Sprite finalFace = npcFace != null ? npcFace : GetSpriteForSpeaker(npcName);

        faceImage.sprite = finalFace;
        nameText.text = npcName;

        faceImage.rectTransform.anchoredPosition = new Vector2(254, 11);
        nameText.rectTransform.anchoredPosition = new Vector2(254, -71);
        dialogueText.rectTransform.anchoredPosition = new Vector2(-57, -3);

        if (dialogueBackgroundImage != null)
            dialogueBackgroundImage.sprite = npcDialogueSprite;

        SetHintPosition(false);
    }

    private void ResetDialogueToPlayer()
    {
        faceImage.sprite = playerFace;
        nameText.text = playerName;

        faceImage.rectTransform.anchoredPosition = new Vector2(-254, 11);
        nameText.rectTransform.anchoredPosition = new Vector2(-254, -71);
        dialogueText.rectTransform.anchoredPosition = new Vector2(57, 3);

        if (dialogueBackgroundImage != null)
            dialogueBackgroundImage.sprite = playerDialogueSprite;

        SetHintPosition(true);
    }

    void ShowCurrentLine()
    {
        if (currentIndex >= lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentIndex];

        if (line.isPlayer)
        {
            ResetDialogueToPlayer();
        }
        else
            ResetDialogueToNPC(line.speakerFace, line.speakerName);

        if (line.choices != null && line.choices.Length > 0)
        {
            ShowChoiceButtons(line.choices);
            dialogueText.text = line.text;
            if (hintText != null) hintText.SetActive(false);
            isTyping = false;
            return;
        }

        dialogueText.gameObject.SetActive(true);
        if (choiceContainer != null) choiceContainer.gameObject.SetActive(false);

        StartCoroutine(TypeLines(line.text));
    }

    private void ShowChoiceButtons(DialogueChoice[] choices)
    {
        isChoiceActive = true;

        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        foreach (var choice in choices)
        {
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceContainer);
            Button btn = btnObj.GetComponent<Button>();
            Text btnText = btn.GetComponentInChildren<Text>();
            btnText.text = choice.buttonText;
            int nextIndex = choice.nextLineIndex;
            btn.onClick.AddListener(() => OnChoiceSelected(nextIndex));

            ButtonHoverColor hover = btnObj.AddComponent<ButtonHoverColor>();
            hover.targetText = btnText;
            hover.hoverColor = new Color(219f/255f, 177f/255f, 111f/255f);
        }
        choiceContainer.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);
        if (hintText != null) hintText.SetActive(false);
    }

    private void OnChoiceSelected(int nextIndex)
    {
        isChoiceActive = false;
        choiceContainer.gameObject.SetActive(false);

        OnChoiceSelectedEvent?.Invoke(currentDialogueKey, currentIndex, nextIndex);

        if (nextIndex == -1)
        {
            if (shopUI != null)
            {
                isShopOpened = true;
                shopUI.OpenShop();
                dialogueBox.SetActive(false);
            }
            else
                Debug.LogError("ShopUI is null!");
            return;
        }
        else if (nextIndex == -2)
        {
            EndDialogue();
        }
        else
        {
            currentIndex = nextIndex;
            ShowCurrentLine();
        }

    }

   private IEnumerator TypeLines(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";
        if (hintText != null) hintText.SetActive(false);
        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        isTyping = false;
        if (hintText != null) StartGlowHint();
    }

    private void ShowNextLine()
    {
        if (currentIndex + 1 >= lines.Count)
        {
            EndDialogue();
            return;
        }
        currentIndex++;
        ShowCurrentLine();
    }

    private void StartGlowHint()
    {
        if (glowCoroutine != null) StopCoroutine(glowCoroutine);

        if (hintText != null)
        {
            Text hint = hintText.GetComponent<Text>();
            if (hint != null)
                hint.color = Color.black;
        }
        glowCoroutine = StartCoroutine(GlowHint());
    }

    private IEnumerator GlowHint()
    {
        Text hint = hintText.GetComponent<Text>();
        if (hint == null) yield break;

        hintText.SetActive(true);

        Color grayColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        Color goldenColor = new Color(219f/255f, 177f/255f, 111f/255f);
        Color blackColor = Color.black;

        float speed = 2.0f;
        float startTime = Time.realtimeSinceStartup;

        while (!isTyping && isDialogueActive)
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            float factor = (Mathf.Sin(elapsed * speed) + 1) / 2f;

            if (factor < 0.5f)
            {
                float t = factor * 2f;
                hint.color = Color.Lerp(blackColor, grayColor, t);
            }
            else
            {
                float t = (factor - 0.5f) * 2f;
                hint.color = Color.Lerp(grayColor, goldenColor, t);
            }
            yield return null;
        }
        hintText.SetActive(false);
        hint.color = blackColor;
    }

    private void SetHintPosition(bool isPlayer)
    {
        if (hintRect == null) return;
        if (isPlayer)
            hintRect.anchoredPosition = new Vector2(37f, -71.865f);
        else
            hintRect.anchoredPosition = new Vector2(-35.56f, -71.44f);
    }

    private Sprite GetSpriteForSpeaker(string speakerName)
    {
        if (speakerName == playerName) return playerFace;
        if (npcSprites.ContainsKey(speakerName))
            return npcSprites[speakerName];
        return null;
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        isChoiceActive = false;
        dialogueBox.SetActive(false);

        if (choiceContainer != null) choiceContainer.gameObject.SetActive(false);
        if (hintText != null) hintText.SetActive(false);
        if (glowCoroutine != null) StopCoroutine(glowCoroutine);

        if (!GameState.IsCutscenePlaying)
        {
            if (Player.Instance != null)
                Player.Instance.SetMovementBlocked(false);

            if (!isShopOpened)
            {
                if (inventoryController != null)
                    inventoryController.enabled = true;
                if (mattock != null) mattock.enabled = true;
                if (wateringCan != null) wateringCan.enabled = true;
            }

        }

        if (!string.IsNullOrEmpty(currentNPCName))
        {
            string taskDesc = GetQuestTaskDescription(currentNPCName);
            if (currentDialogueKey != "IhvilnichtDialogueQuest5" && currentDialogueKey != "DialogueQuest10")
                QuestManager.Instance.CompleteTask(taskDesc);
        }

        if (currentDialogueKey == "TioliDialogueQuest2" && currentNPCName == "Тиоли")
        {
            if (tutorialHomeController != null)
                tutorialHomeController.ShowPanel();

            if (houseTriggerZone != null)
                houseTriggerZone.SetActive(true);
        }

        if (currentDialogueKey == "TioliDialogueQuest4" && currentNPCName == "Тиоли")
        {
            QuestManager.Instance.CompleteTask("Навестить Тиоли");
            QuestManager.Instance.CompleteTask("Попытаться выяснить, что её тревожит");
        }

        OnDialogueEnded?.Invoke(currentNPCName);
        Canvas.ForceUpdateCanvases();
    }

    public void OnShopClosed()
    {
        isShopOpened = false;

        if (inventoryController != null) inventoryController.enabled = true;
        if (mattock != null) mattock.enabled = true;
        if (wateringCan != null) wateringCan.enabled = true;
        if (Player.Instance != null) Player.Instance.SetMovementBlocked(false);
    }

    private string GetQuestTaskDescription(string npcName)
    {
        switch (npcName)
        {
            case "Терентий": return "Поговорить с Терентием";
            case "Финник": return "Поговорить с Финником";
            case "Ихвильнихт": return "Поговорить с Ихвильнихтом";
            case "Тиоли": return "Поговорить с Тиоли";
            default: return $"Поговорить с {npcName}";
        }
    }
}
