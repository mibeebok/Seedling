using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public string dialogueKey;
    public string npcName;

    public GameObject TextE;
    
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TextE.SetActive(false);

            dialogueManager.StartDialogueByKey(dialogueKey, npcName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!CutsceneManager.IsPlaying && TextE != null)
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

}
