using UnityEngine;

public class TriggerZoneQ14_2 : MonoBehaviour
{
    public FinalButtonController finalButtonController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finalButtonController != null)
                finalButtonController.EnableFinalButton();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (finalButtonController != null)
                finalButtonController.DisableFinalButton();
        }
    }
}
