using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Seed,
    Vegetable,
    Tool
}
[System.Serializable]
public class Item
{
    public int id;
    public string name;
    public Sprite img;
    public int maxStack = 32;
    public ItemType type;
    public CropType cropType;
}

public class DataBase : MonoBehaviour
{
    public static DataBase Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public List<Seed> seeds = new List<Seed>();
    public List<Vegetable> vegetables = new List<Vegetable>();
    public Seed GetSeed(CropType cropType)
    {
        return seeds.Find(seed => seed.cropType == cropType);
    }
    public Vegetable GetVegetable(CropType cropType)
    {
        return vegetables.Find(veg => veg.cropType == cropType);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (items.Count == 0 || items[0].id != 0)
            {
                items.Insert(0, new Item
                {
                    id = 0,
                    name = "Empty",
                    img = null,
                    maxStack = 1
                });
            }
        }
        else
        {
            return;
        }
    }

    public Item GetItemById(int id)
    {
        foreach (Item item in items)
        {
            if (item.id == id) return item;
        }
        return null;
    }
    public Item GetItemByCropType(CropType type, bool isSeed)
    {
        return items.Find(item =>
            item.cropType == type &&
            item.type == (isSeed ? ItemType.Seed : ItemType.Vegetable));
    }
    void Start()
    {
        DebugAllItems();
    }
    public void DebugAllItems()
    {
        Debug.Log("=== Все предметы в базе ===");
        foreach (Item item in items)
        {
            Debug.Log($"ID: {item.id}, Name: {item.name}, Type: {item.type}, CropType: {item.cropType}");
        }
    }
}