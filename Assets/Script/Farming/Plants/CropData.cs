using UnityEngine;

public abstract class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthStages;
    public float growTime = 5f;

    // вызывается каждый раз при росте
    public abstract void DoGrowth(CropBehaviour crop);
}
