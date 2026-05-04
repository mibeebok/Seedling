using UnityEngine;

public class SoilTile : MonoBehaviour
{
    [Header("State")]
    public bool isPlowed = false;
    public bool isWatered = false;
    public bool wasWateredYesterday = false;
    public bool isPlanted = false;
    public int daysWithoutWater = 0;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite plowedSprite;
    public Sprite plowedDrySprite;
    public Sprite wateredSprite;
    public Sprite wateredYesterdaySprite;

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private SoilTileWateringCan wateringCan;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wateringCan = GetComponent<SoilTileWateringCan>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.sortingLayerName = "Default";
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (isWatered && wateredSprite != null)
        {
            // Полита СЕГОДНЯ
            spriteRenderer.sprite = wateredSprite;
        }
        else if (wasWateredYesterday && wateredYesterdaySprite != null)
        {
            // Полита ВЧЕРА
            spriteRenderer.sprite = wateredYesterdaySprite;
        }
        else if (isPlowed && plowedSprite != null)
        {
            // Просто вспахана
            spriteRenderer.sprite = plowedSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    public void Plow()
    {
        isPlowed = true;
        isWatered = false;
        wasWateredYesterday = false;
        daysWithoutWater = 0;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void Water()
    {
        isWatered = true;
        wasWateredYesterday = false;
        daysWithoutWater = 0;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public bool IsReadyForPlanting()
    {
        return isPlowed && isWatered && !isPlanted;
    }

    public void MarkPlanted()
    {
        isPlanted = true;
        SaveSystem.MarkTileChanged(GetGridPos());
    }
    
    public void ClearPlanted()
    {
        isPlanted = false;
        isWatered = false;
        wasWateredYesterday = false;
        daysWithoutWater = 0;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void MarkHarvested()
    {
        isPlanted = false;
        isWatered = false;
        wasWateredYesterday = false;
        daysWithoutWater = 0;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void ClearDailyWater()
    {
        if (isWatered)
        {
            wasWateredYesterday = true;
        }
        else
        {
            wasWateredYesterday = false;
        }
        
        isWatered = false;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void DryOut()
    {
        if (isPlowed && !isPlanted)
        {
            if (!isWatered && !wasWateredYesterday)
            {
                isPlowed = false;
                wasWateredYesterday = false;
                daysWithoutWater = 0;
                UpdateVisual();
                SaveSystem.MarkTileChanged(GetGridPos());
            }
        }
    }
    public SaveData GetSaveData()
    {
        return new SaveData
        {
            position = transform.position,
            isPlowed = isPlowed,
            isWatered = isWatered,
            isPlanted = isPlanted,
            daysWithoutWater = daysWithoutWater,
            wasWateredYesterday = wasWateredYesterday
        };
    }

    public void LoadFromSaveData(SaveData data)
    {
        isPlowed = data.isPlowed;
        isWatered = data.isWatered;
        isPlanted = data.isPlanted;
        daysWithoutWater = data.daysWithoutWater;
        wasWateredYesterday = data.wasWateredYesterday;
        UpdateVisual();
    }

    private Vector2Int GetGridPos()
    {
        return FarmGrid.Instance.WorldToGridPosition(transform.position);
    }
}