using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector2 position;
    public bool isPlowed;
    public bool isWatered;
    public bool isPlanted;
    public int daysWithoutWater;
    public bool wasWateredYesterday;
}