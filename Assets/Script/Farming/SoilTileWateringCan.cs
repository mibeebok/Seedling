using UnityEngine;

public class SoilTileWateringCan : MonoBehaviour
{
    private SoilTile soil;

    private void Awake()
    {
        soil = GetComponent<SoilTile>();
    }

    public void Water()
    {
        if (soil != null)
        {
            soil.Water();
        }
    }

    public static void ResetAllWateredTiles()
    {
        var all = Object.FindObjectsOfType<SoilTile>();
        foreach (var tile in all)
        {
            tile.ClearDailyWater(); // Этот метод теперь в SoilTile
        }
        Debug.Log($"Сброшен полив для {all.Length} тайлов");
    }
}
