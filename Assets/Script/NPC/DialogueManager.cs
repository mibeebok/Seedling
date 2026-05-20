using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public Image dialogueBackgroundImage;
    public Image faceImage;
    public Text nameText;
    public Text dialogueText;

    public Sprite npcDialogueSprite;
    public Sprite playerDialogueSprite;

    private InventoryController inventoryController;

    public Sprite playerFace;
    public string playerName;

    public GameObject choicePanel;
    public Button dialogueChoiceButton;
    public Button shopChoiceButton;

    public ShopUI shopUI;

    private List<DialogueLine> lines;
    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    private List<DialogueLine> pendingLines;
    private string pendingNpcName;
    private Sprite pendingNpcFace;


    private void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }
    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[currentIndex].text;
                isTyping = false;
            }
            else
            {
                ShowNextLine();
            }
                
        }
    }

    public void StartDialogue(List<DialogueLine> dialogueLines, string npcName, Sprite npcFace)
    {

        if (npcName == "Терентий")
        {
            pendingLines = dialogueLines;
            pendingNpcName = npcName;
            pendingNpcFace = npcFace;

            ResetDialogueToNPC(npcFace, npcName);

            dialogueBox.SetActive(true);
            dialogueText.gameObject.SetActive(false);

            choicePanel.SetActive(true);

            dialogueChoiceButton.onClick.RemoveAllListeners();
            dialogueChoiceButton.onClick.AddListener(OnDialogueChoice);

            shopChoiceButton.onClick.RemoveAllListeners();
            shopChoiceButton.onClick.AddListener(OnShopChoice);
            return;
        }

        StartNormalDialogue(dialogueLines, npcName, npcFace);
    }

    private void ResetDialogueToNPC(Sprite npcFace, string npcName)
    {
        faceImage.sprite = npcFace;
        nameText.text = npcName;

        faceImage.rectTransform.anchoredPosition = new Vector2(254, 11);
        nameText.rectTransform.anchoredPosition = new Vector2(254, -71);
        dialogueText.rectTransform.anchoredPosition = new Vector2(-57, -3);

        if (dialogueBackgroundImage != null)
            dialogueBackgroundImage.sprite = npcDialogueSprite;

        faceImage.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);

        dialogueText.gameObject.SetActive(false);
    }

    private void StartNormalDialogue(List<DialogueLine> dialogueLines, string npcName, Sprite npcFace)
    {

        lines = dialogueLines;
        currentIndex = 0;
        isDialogueActive = true;
        dialogueBox.SetActive(true);

        if (Player.Instance != null)
            Player.Instance.SetMovementBlocked(true);

        if (inventoryController != null)
            inventoryController.enabled = false;

        for (int i = 0; i < lines.Count; i++)
        {
            if (!lines[i].isPlayer)
            {
                lines[i].speakerName = npcName;
                lines[i].speakerFace = npcFace;
            }
        }

        ShowCurrentLine();
    }

    private void OnDialogueChoice()
    {
        choicePanel.SetActive(false);
        dialogueText.gameObject.SetActive(true);

        StartNormalDialogue(pendingLines, pendingNpcName, pendingNpcFace);
    }

    private void OnShopChoice()
    {
        choicePanel.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        OpenShop();
    }

    private void OpenShop()
    {
        if (shopUI != null)
            shopUI.OpenShop();
        else
            Debug.LogError("ShopUI не назначен в DialogueManager!");
    }

    void ShowCurrentLine()
    {
        if (currentIndex >= lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentIndex];

        if (dialogueBackgroundImage != null)
        {
            dialogueBackgroundImage.sprite = line.isPlayer ? playerDialogueSprite : npcDialogueSprite;
        }

        faceImage.sprite = line.isPlayer ? playerFace : line.speakerFace;

        nameText.text = line.isPlayer ? playerName : line.speakerName;

        dialogueText.text = line.text;

        if (line.isPlayer)
        {
            faceImage.rectTransform.anchoredPosition = new Vector2(-254, 11);
            nameText.rectTransform.anchoredPosition = new Vector2(-254, -71);
            dialogueText.rectTransform.anchoredPosition = new Vector2(57, 3);
            
        }
        else
        {
            faceImage.rectTransform.anchoredPosition = new Vector2(254, 11);
            nameText.rectTransform.anchoredPosition = new Vector2(254, -71);
            dialogueText.rectTransform.anchoredPosition = new Vector2(-57, -3);

        }

        StartCoroutine(TypeLines(line.text));
    }

    IEnumerator TypeLines(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }

    void ShowNextLine()
    {
        currentIndex++;
        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBox.SetActive(false);

        if (choicePanel != null) choicePanel.SetActive(false);

        pendingLines = null;
        pendingNpcName = null;
        pendingNpcFace = null;

        if (Player.Instance != null)
            Player.Instance.SetMovementBlocked(false);

        if (inventoryController != null)
            inventoryController.enabled = true;
    }
}
