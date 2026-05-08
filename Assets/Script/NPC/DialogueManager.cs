using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public Image dialogueBackgroundImage;
    public Image faceImage;
    public Text nameText;
    public Text dialogueText;

    public Sprite npcDialogueSprite;
    public Sprite playerDialogueSprite;

    public Sprite playerFace;
    public string playerName;

    private List<DialogueLine> lines;
    private int currentIndex = 0;
    private bool isDialogueActive = false;

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            ShowNextLine();
        }
    }

    public void StartDialogue(List<DialogueLine> dialogueLines, string npcName, Sprite npcFace)
    {
        lines = dialogueLines;
        currentIndex = 0;
        isDialogueActive = true;
        dialogueBox.SetActive(true);

        // ��������� ��� � ���� NPC �� ��� ��� �������
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

    void ShowCurrentLine()
    {
        if (currentIndex >= lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines[currentIndex];

        // ����� ����
        if (dialogueBackgroundImage != null)
        {
            dialogueBackgroundImage.sprite = line.isPlayer ? playerDialogueSprite : npcDialogueSprite;
        }

        // ����
        faceImage.sprite = line.isPlayer ? playerFace : line.speakerFace;

        // ���
        nameText.text = line.isPlayer ? playerName : line.speakerName;

        // �����
        dialogueText.text = line.text;

        // ������ ������������ UI (����/�����)
        if (line.isPlayer)
        {
            // ������� ������ � �����
            faceImage.rectTransform.anchoredPosition = new Vector2(-254, 11);
            nameText.rectTransform.anchoredPosition = new Vector2(-254, -71);
            dialogueText.rectTransform.anchoredPosition = new Vector2(57, 3);
            
        }
        else
        {
            // ������� NPC � ������
            faceImage.rectTransform.anchoredPosition = new Vector2(254, 11);
            nameText.rectTransform.anchoredPosition = new Vector2(254, -71);
            dialogueText.rectTransform.anchoredPosition = new Vector2(-57, -3);

        }
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
    }
}
