using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public DataBase data;
    public List<ItemInventory> items = new List<ItemInventory>();
    public GameObject gameObjectShow;
    public GameObject InventoryMainObject;
    public int maxCount;

    public EventSystem es;
    public int currentID = -1;
    public ItemInventory currentItem;
    public RectTransform movingObject;
    public Vector3 offset;

    private const int TOOL_SLOTS = 2;

    private void Start() => InitializeInventory();

    private void Update()
    {
        if (currentID != -1 && movingObject != null)
            movingObject.position = Input.mousePosition + offset;
    }

    public void InitializeInventory()
    {
        if (items.Count == 0)
        {
            ClearInventory();
            AddGraphics();
            
            for (int i = 0; i < maxCount; i++)
            {
                if (i < TOOL_SLOTS)
                    AddItemSilent(i, data.items[i + 1]); // Лейка и мотыга
                else
                    AddItem(i, data.items[0], 0);
            }
        }
        UpdateInventory();
    }

    public void AddItem(int id, Item item, int count)
    {
        if (id < TOOL_SLOTS) return;
        
        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.img;

        var text = items[id].itemGameObject.GetComponentInChildren<Text>();
        text.text = (count > 1 && item.id != 0) ? count.ToString() : "";
    }

    private void AddItemSilent(int id, Item item)
    {
        items[id].id = item.id;
        items[id].count = 1;
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        if (id < TOOL_SLOTS) return;
        
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].img;

        var text = items[id].itemGameObject.GetComponentInChildren<Text>();
        text.text = (invItem.count > 1 && invItem.id != 0) ? invItem.count.ToString() : "";
    }

    public void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjectShow, InventoryMainObject.transform);
            newItem.name = i.ToString();

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;

            float xPos = (i % 8) * (rt.rect.width + 5);
            float yPos = -(i / 8) * (rt.rect.height + 5);
            rt.anchoredPosition = new Vector2(xPos, yPos);

            ItemInventory ii = new ItemInventory
            {
                id = 0,
                itemGameObject = newItem,
                count = 0
            };

            Button tempButton = newItem.GetComponent<Button>();
            tempButton.onClick.AddListener(SelectObject);

            Image itemImage = newItem.GetComponent<Image>();
            Text countText = newItem.GetComponentInChildren<Text>();

            if (i < TOOL_SLOTS)
            {
                itemImage.sprite = data.items[i + 1]?.img;
                itemImage.enabled = true;
                itemImage.color = Color.white;
                itemImage.preserveAspect = true;
                itemImage.SetNativeSize(); 
                RectTransform imgRt = itemImage.GetComponent<RectTransform>();
                if (imgRt != null)
                {
                    imgRt.sizeDelta = new Vector2(rt.rect.width * 0.8f, rt.rect.height * 0.8f);
                }
                
                if (countText != null) countText.text = "";
                tempButton.interactable = false;
            }
            else
            {
                itemImage.sprite = data.items[0].img;
                itemImage.enabled = true;
                itemImage.color = Color.white;
                itemImage.preserveAspect = false; // Обычные предметы пусть масштабируются
                if (countText != null) countText.text = "";
                tempButton.interactable = true;
            }

            items.Add(ii);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            var text = items[i].itemGameObject.GetComponentInChildren<Text>();
            text.text = (items[i].id != 0 && items[i].count > 1) ? items[i].count.ToString() : "";

            Image img = items[i].itemGameObject.GetComponent<Image>();
            
            if (i < TOOL_SLOTS)
            {
                img.sprite = data.items[i + 1]?.img;
                img.preserveAspect = true;
            }
            else
            {
                img.sprite = data.items[items[i].id].img;
                img.preserveAspect = false;
            }
            
            img.enabled = true;
        }
    }

    public void SelectObject()
    {
        GameObject selected = es.currentSelectedGameObject;
        if (selected == null) return;
        
        int slotIndex = int.Parse(selected.name);
        
        if (slotIndex < TOOL_SLOTS) return; // Инструменты не трогаем
        
        if (currentID == -1)
        {
            if (items[slotIndex].id == 0) return;
            
            currentID = slotIndex;
            currentItem = CloneItem(items[currentID]);
            movingObject.gameObject.SetActive(true);
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;
            AddItem(currentID, data.items[0], 0);
        }
        else
        {
            if (slotIndex < TOOL_SLOTS)
            {
                AddItem(currentID, data.items[currentItem.id], currentItem.count);
                currentID = -1;
                movingObject.gameObject.SetActive(false);
                return;
            }
            
            AddInventoryItem(currentID, items[slotIndex]);
            AddInventoryItem(slotIndex, currentItem);
            currentID = -1;
            movingObject.gameObject.SetActive(false);
        }
    }

    public void ClearInventory()
    {
        foreach (Transform child in InventoryMainObject.transform)
            Destroy(child.gameObject);
        items.Clear();
    }

    public bool AddItemToFirstFreeSlot(Item item, int count)
    {
        for (int i = TOOL_SLOTS; i < items.Count; i++)
        {
            if (items[i].id == 0)
            {
                AddItem(i, item, count);
                return true;
            }
        }
        return false;
    }

    private static ItemInventory CloneItem(ItemInventory src) => new ItemInventory
    {
        id = src.id,
        itemGameObject = src.itemGameObject,
        count = src.count
    };
}

[System.Serializable]
public class ItemInventory
{
    public int id;
    public GameObject itemGameObject;
    public int count;
}