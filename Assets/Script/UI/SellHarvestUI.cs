using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;

public class SellHarvestUI : MonoBehaviour
{
    public GameObject sellHarvestPanel;
    public GameObject confirmPanel;

    public Transform itemsContainer;
    public GameObject itemSlotPrefab;

    public Button closeButton;
    public Button sellAllButton;

    public Text confirmMessage;
    public Button confirmCloseButton;
    public GameObject emptyMessage;

    private List<SellItemsSlotUI> currentSlots = new List<SellItemsSlotUI>();
    private bool isInitialized = false;

    void Start()
    {
        if (closeButton) closeButton.onClick.AddListener(ClosePanel);
        if (sellAllButton) sellAllButton.onClick.AddListener(OnSellAllClicked);
        if (confirmCloseButton) confirmCloseButton.onClick.AddListener(() => confirmPanel.SetActive(false));

        CreateSlotsOnce();

        sellHarvestPanel.SetActive(false);
        confirmPanel.SetActive(false);
    }

  
    private void UpdateSlotsCounters()
    {
        var vegetables = GetVegetablesFromInventory();

        ShowEmptyMessageIfNeeded();

        foreach (var slot in currentSlots)
        {
            var veg = vegetables.Find(v => v.item.id == slot.GetItemId());
            if (veg.availableCount > 0)
            {
                slot.UpdateCount(veg.availableCount);
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }

        foreach (var veg in vegetables)
        {
            bool exists = currentSlots.Exists(slot => slot.GetItemId() == veg.item.id);
            if (!exists)
            {
                GameObject slotObj = Instantiate(itemSlotPrefab, itemsContainer);
                SellItemsSlotUI slotUI = slotObj.GetComponent<SellItemsSlotUI>();
                if (slotUI == null) slotUI = slotObj.AddComponent<SellItemsSlotUI>();
                slotUI.Initialize(veg.item, veg.availableCount, this);
                currentSlots.Add(slotUI);
            }
        }
    }
    private void CreateSlotsOnce()
    {
        foreach (var slot in currentSlots)
                if (slot != null) Destroy(slot.gameObject);
        currentSlots.Clear();

        var vegetables = GetVegetablesFromInventory();
        foreach (var veg in vegetables)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemsContainer);
            SellItemsSlotUI slotUI = slotObj.GetComponent<SellItemsSlotUI>();
            if (slotUI == null) slotUI = slotObj.AddComponent<SellItemsSlotUI>();
            slotUI.Initialize(veg.item, veg.availableCount, this);
            currentSlots.Add(slotUI);
        }

        ShowEmptyMessageIfNeeded();
    }
    public void OpenSellPanel()
    {
        UpdateSlotsCounters();
        sellHarvestPanel.SetActive(true);
    }

    public void UpdateSlots()
    {
        UpdateSlotsCounters();
    }

    void ClosePanel()
    {
        sellHarvestPanel.SetActive(false);
    }
    public struct VegetableStock
    {
        public Item item;
        public int availableCount;
    }

    private List<VegetableStock> GetVegetablesFromInventory()
    {
        List<VegetableStock> result = new List<VegetableStock>();
        InventoryController invController = InventoryController.Instance;
        if (invController == null || invController.mainInventory == null) return result;

        DataBase db = DataBase.Instance;

        var itemsList = invController.mainInventory.items;

        for (int i = InventoryController.TOOL_SLOTS; i < itemsList.Count; i++)
        {
            int id = itemsList[i].id;
            if (id == 0) continue;

            Item item = db.GetItemById(id);
            if (item != null && item.type == ItemType.Vegetable && item.cropType != CropType.None)
            {
                int count = itemsList[i].count;
                if (count > 0)
                {
                    result.Add(new VegetableStock { item = item, availableCount = count });
                }
            }
        }
        return result;
    }

    public void SellVegetable(Item item, int amount)
    {
        if (amount <= 0) return;

        bool success = InventoryController.Instance.RemoveItem(item, amount);
        if (!success)
        {
            ShowConfirmMessage($"Ошибка: недостаточно {GetRussianName(item)} в инвентаре!");
            return;
        }

        int totalMoney = GetSellPrice(item.cropType) * amount;
        MoneyDisplay.Instance.AddMoney(totalMoney);
        UpdateShopMoneyDisplay();

        string vegName = GetRussianName(item);
        string message = $"Вы продали {vegName} в количестве: {amount} шт.\nИ получили с этого: {totalMoney} листеньев!";
        ShowConfirmMessage(message);
        QuestManager.Instance.CompleteTask("Продать часть урожая");

        UpdateSlotsCounters();

        ShowEmptyMessageIfNeeded();
    }

    private string GetRussianName(Item item)
    {
        switch (item.cropType)
        {
            case CropType.Potato: return "картофель";
            case CropType.Beetroot: return "свекла";
            case CropType.Carrot: return "морковь";
            case CropType.Rastberry: return "малина";
            default: return item.name;
        }
    }

    public void SellAllVegetables()
    {
        var vegetables = GetVegetablesFromInventory();
        if (vegetables.Count == 0)
        {
            ShowConfirmMessage("Нет овощей для продажи!");
            return;
        }

        int totalMoney = 0;
        List<string> soldItemsList = new List<string>();

        foreach (var veg in vegetables)
        {
            int price = GetSellPrice(veg.item.cropType);
            int moneyForThis = price * veg.availableCount;
            totalMoney += moneyForThis;
            string vegName = GetRussianName(veg.item);
            soldItemsList.Add($"{vegName} в количестве {veg.availableCount} шт.");
            InventoryController.Instance.RemoveItem(veg.item, veg.availableCount);
        }

        MoneyDisplay.Instance.AddMoney(totalMoney);
        UpdateShopMoneyDisplay();

        string soldItems = string.Join(", ", soldItemsList);
        string message = $"Вы продали: {soldItems}\nИ получили с этого: {totalMoney} листеньев!";
        ShowConfirmMessage(message);
        QuestManager.Instance.CompleteTask("Продать часть урожая");

        UpdateSlotsCounters();

        ShowEmptyMessageIfNeeded();
    }

    private void OnSellAllClicked()
    {
        SellAllVegetables();
    }

    private int GetSellPrice(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Potato: return 8;
            case CropType.Carrot: return 10;
            case CropType.Beetroot: return 11;
            case CropType.Rastberry: return 14;
            default: return 5;
        }
    }

    public void ShowConfirmMessage(string msg)
    {
        if (confirmMessage != null)
            confirmMessage.text = msg;
        confirmPanel.transform.SetAsLastSibling();
        if (confirmPanel != null)
            confirmPanel.SetActive(true);
    }

    private void ShowEmptyMessageIfNeeded()
    {
        bool hasVegetables = GetVegetablesFromInventory().Count > 0;
        if (emptyMessage != null)
            emptyMessage.SetActive(!hasVegetables);

        if (itemsContainer != null)
            itemsContainer.gameObject.SetActive(hasVegetables);
    }

    private void UpdateShopMoneyDisplay()
    {
        ShopUI shop = FindFirstObjectByType<ShopUI>();
        if (shop != null && shop.shopMoneyDisplay != null && MoneyDisplay.Instance != null)
            shop.shopMoneyDisplay.UpdateMoneyDisplay(MoneyDisplay.Instance.GetMoney());
    }
}
