using UnityEngine;
using UnityEngine.Tilemaps;

public class CropBehaviour : MonoBehaviour
{
    [Header("Crop Data")]
    public Crop cropData; // ScriptableObject с данными о растении

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void Grow()
    {
        if (cropData == null || cropData.growthStages == null || cropData.growthStages.Length == 0) return;

        if (currentStage < cropData.growthStages.Length - 1)
        {
            currentStage++;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        if (cropData == null || cropData.growthStages == null || cropData.growthStages.Length == 0) return;
        TileBase tileBase = cropData.growthStages[currentStage];
        if (tileBase == null) return;

        // Попробуем привести к Tile и взять sprite
        var tile = tileBase as Tile;
        if (tile != null)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = tile.sprite;
            return;
        }

        // Если в growthStages вдруг положены ScriptableObject-ы другого типа, можно обработать это здесь
        Debug.LogWarning("CropBehaviour: growthStages[currentStage] не является Tile с sprite.");
    }

    public Item Harvest()
    {
        return cropData?.harvestItem;
    }
}
