using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattockController : Sounds
{
    [Header("References")]
    public InventoryController inventoryController;
    public Animator handsAnimator;
    public Transform playerTransform;
    public DialogueManager dialogueManager;

    [Header("Mattock Setting")]
    public int mattockItemId = 2;
    public string mattockBool = "Mattock";
    public float soilMattockDelay = 0.4f;
    public float interactionRadius = 3.5f;

    [Header("Sprites")]
    public Sprite plowedSprite;

    [Header("Настройки")]
    public LayerMask obstacleMask = ~0;

    private bool isMattock = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryMattockSoil();
        }
    }

    void TryMattockSoil()
    {
        if (inventoryController == null || inventoryController.GetSelectedSlot() != 1) 
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(playerTransform.position, mousePos) > interactionRadius)
            return;

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mousePos, 1.5f);
        bool isOccupied = false;

        foreach (var col in nearbyColliders)
        {
            if (col.CompareTag("Obstacle"))
            {
                isOccupied = true;
                break;
            }
        }

        
        if (isOccupied)
        {
            NPCInteraction npc = FindObjectOfType<NPCInteraction>();
            if (npc != null && npc.dialogueManager != null)
            {
                List<DialogueLine> lines = new List<DialogueLine>
                {
                    new DialogueLine { text = "Я не буду вспахивать тут землю. Здесь уже растет прекрасное растение.", isPlayer = true }
                };
                npc.dialogueManager.StartDialogueFromCode(lines, "Гриша");
            }
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        
        if (hit.collider != null)
        {
            SoilTile soilTile = hit.collider.GetComponent<SoilTile>() ?? 
                                hit.collider.GetComponentInParent<SoilTile>();

            if (soilTile != null)
            {
                if (handsAnimator != null && !isMattock)
                {
                    PlaySound(sounds[0], volume: 0.3f, p1: 0.9f, p2: 1.2f);
                    handsAnimator.SetBool(mattockBool, true);
                    StartCoroutine(ResetMattockBool());
                }
                soilTile.Plow();
            }
        }
    }

    IEnumerator ResetMattockBool()
    {
        isMattock = true;
        AnimationClip mattockClip = GetAnimationClipByName("Mattock");
        float duration = mattockClip != null ? mattockClip.length : 1f;
        yield return new WaitForSeconds(duration);
        handsAnimator.SetBool(mattockBool, false);
        isMattock = false;
    }

    AnimationClip GetAnimationClipByName(string name)
    {
        RuntimeAnimatorController ac = handsAnimator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
}