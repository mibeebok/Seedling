using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CropType cropType = CropType.Potato;
    public bool isSeed = true;
    public int amount = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (isSeed)
            InventoryManager.Instance?.AddSeedToInventory(cropType, amount);
        else
            InventoryManager.Instance?.AddVegetableToInventory(cropType, amount);

        Destroy(gameObject);
    }
}