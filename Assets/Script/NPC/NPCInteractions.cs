using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string dialogueKey;
    public string npcName;

    public GameObject TextE;
    
    private bool playerInRange = false;

    private void Start()
    {
        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded += OnDialogueEndedHandler;
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
            dialogueManager.OnDialogueEnded -= OnDialogueEndedHandler;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(dialogueKey))
            {
                TextE.SetActive(false);
                dialogueManager.StartDialogueByKey(dialogueKey, npcName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!GameState.IsCutscenePlaying && !string.IsNullOrEmpty(dialogueKey) && TextE != null)
            TextE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TextE.SetActive(false);
            playerInRange = false;
            if (dialogueManager != null)
                dialogueManager.EndDialogue(); 
        }
    }

    public void RefreshTextE()
    {
        if (TextE != null)
            TextE.SetActive(playerInRange);
    }

    private void OnDialogueEndedHandler(string endedNPCName)
    {
        if (endedNPCName == npcName && !string.IsNullOrEmpty(dialogueKey) &&
            (dialogueKey == "TerentyDialogueQuest1" ||
            dialogueKey == "FinnickDialogueQuest1" ||
            dialogueKey == "IhvilnichtDialogueQuest1" ||
            dialogueKey == "Intro"))
        {
            dialogueKey = null;
            if (TextE != null) TextE.SetActive(false);
        }
    }

}
