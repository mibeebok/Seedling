using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddSeedToInventory(CropType cropType, int amount)
    {
        Item seedItem = DataBase.Instance.GetItemByCropType(cropType, isSeed: true);
        if (seedItem != null)
            InventoryController.Instance.AddItem(seedItem, amount);
        else
            Debug.LogError($"Seed item not found for {cropType}");
    }

    public void AddVegetableToInventory(CropType cropType, int amount)
    {
        Item vegItem = DataBase.Instance.GetItemByCropType(cropType, isSeed: false);
        if (vegItem != null)
            InventoryController.Instance.AddItem(vegItem, amount);
        else
            Debug.LogError($"Vegetable item not found for {cropType}");
    }
}