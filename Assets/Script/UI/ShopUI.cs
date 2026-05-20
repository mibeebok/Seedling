using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public static bool IsShopOpen { get; private set; } = false;

    public GameObject shopWindow;
    public Transform itemsContainer;
    public GameObject shopItemSlotPrefab;
    public ShopItem[] shopItems;

    public GameObject quantityPanel;
    public Text quantityText;
    public Text totalCostText;
    public Button minusButton;
    public Button plusButton;
    public Button okButton;
    public Text itemNameText;
    public Button closeQuantityButton;

    public GameObject errorPanel;
    public Button errorOkButton;

    private int currentQuantity = 1;
    private ShopItem currentSelectedItem;

    private MattockController mattock;
    private WateringCanController wateringCan;
    private InventoryController inventoryController;

    public ShopMoneyDisplay shopMoneyDisplay;
    private MoneyDisplay globalMoneyDisplay;


    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
        public Sprite icon;
        public CropType cropType;
    }

    private void Start()
    {
        mattock = FindObjectOfType<MattockController>();
        wateringCan = FindObjectOfType<WateringCanController>();
        inventoryController = FindObjectOfType<InventoryController>();

        globalMoneyDisplay = FindObjectOfType<MoneyDisplay>();

        if (shopMoneyDisplay == null)
            shopMoneyDisplay = GetComponentInChildren<ShopMoneyDisplay>();

        if (shopWindow != null) shopWindow.SetActive(false);
        if (itemsContainer == null || shopItemSlotPrefab == null)
        {
            Debug.LogError("ShopUI: не назначены itemsContainer или shopItemSlotPrefab");
            return;
        }

        if (quantityPanel != null)
        {
            quantityPanel.SetActive(false);
            if (minusButton != null) minusButton.onClick.AddListener(() => ChangeQuantity(-1));
            if (plusButton != null) plusButton.onClick.AddListener(() => ChangeQuantity(1));
            if (okButton != null) okButton.onClick.AddListener(ConfirmPurchase);
            if (closeQuantityButton != null) closeQuantityButton.onClick.AddListener(CloseQuantityPanel);
        }

        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
            if (errorOkButton != null) errorOkButton.onClick.AddListener(() => errorPanel.SetActive(false));
        }

        if (globalMoneyDisplay == null) Debug.LogError("globalMoneyDisplay not found!");
        if (shopMoneyDisplay == null) Debug.LogError("shopMoneyDisplay not found!");
    }

    public void OpenShop()
    {
        IsShopOpen = true;

        EnableMovementScripts(false);
        if (Player.Instance != null) Player.Instance.SetMovementBlocked(true);

        if (shopWindow != null) shopWindow.SetActive(true);

        if (shopMoneyDisplay != null && globalMoneyDisplay != null)
        {
            int currentCash = globalMoneyDisplay.GetMoney();
            shopMoneyDisplay.UpdateMoneyDisplay(currentCash);
        }
        else
        {
            Debug.LogError($"shopMoneyDisplay is null? {shopMoneyDisplay == null}, globalMoneyDisplay null? {globalMoneyDisplay == null}");
        }

            PopulateShop();
    }

    private void PopulateShop()
    {
        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        foreach (ShopItem item in shopItems)
        {
            GameObject slot = Instantiate(shopItemSlotPrefab, itemsContainer);

            slot.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
            slot.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;
            slot.transform.Find("ItemPrice").GetComponent<Text>().text = $"Цена: {item.price} листеньев";

            Transform soon = slot.transform.Find("ItemSoon");
            if (soon != null) soon.gameObject.SetActive(false);

            Button buyBtn = slot.transform.Find("BuyButton").GetComponent<Button>();

            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() => OpenQuantityPanel(item));
            buyBtn.gameObject.SetActive(true);
        }

        int totalSlots = 6;
        int currentSlots = shopItems.Length;
        for (int i = 0; i < totalSlots - currentSlots; i++)
        {
            GameObject emptySlot = Instantiate(shopItemSlotPrefab, itemsContainer);

            Transform soon = emptySlot.transform.Find("ItemSoon");
            if (soon != null) soon.gameObject.SetActive(true);

            Transform icon = emptySlot.transform.Find("ItemIcon");
            if (icon != null) icon.gameObject.SetActive(false);

            Transform nameText = emptySlot.transform.Find("ItemName");
            if (nameText != null) nameText.gameObject.SetActive(false);

            Transform price = emptySlot.transform.Find("ItemPrice");
            if (price != null) price.gameObject.SetActive(false);

            Button buyBtn = emptySlot.transform.Find("BuyButton").GetComponent<Button>();
            if (buyBtn != null) buyBtn.gameObject.SetActive(false);

        }
    }

    private void OpenQuantityPanel(ShopItem item)
    {
        currentSelectedItem = item;
        currentQuantity = 1;
        UpdateQuantityUI();
        if (quantityPanel != null) quantityPanel.SetActive(true);
        if (itemNameText != null) itemNameText.text = item.itemName;
        UpdateTotalCost();
    }

    private void ChangeQuantity(int delta)
    {
        int newQuantity = currentQuantity + delta;
        if (newQuantity < 1) newQuantity = 1;
        if (newQuantity > 99) newQuantity = 99;
        currentQuantity = newQuantity;
        UpdateQuantityUI();
        UpdateTotalCost();
    }

    private void UpdateQuantityUI()
    {
        if (quantityText != null) quantityText.text = currentQuantity.ToString();
    }

    private void UpdateTotalCost()
    {
        if (totalCostText != null && currentSelectedItem != null)
        {
            int total = currentSelectedItem.price * currentQuantity;
            totalCostText.text = $"Итого: {total}";
        }
    }

    private void ConfirmPurchase()
    {
        if (currentSelectedItem == null) return;
        int totalCost = currentSelectedItem.price * currentQuantity;
        int playerMoney = GetPlayerMoney();

        if (playerMoney >= totalCost)
        {
            if (globalMoneyDisplay != null)
                globalMoneyDisplay.SubtractMoney(totalCost);
            if (shopMoneyDisplay != null)
                shopMoneyDisplay.UpdateMoneyDisplay(globalMoneyDisplay.GetMoney());

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddSeedToInventory(currentSelectedItem.cropType, currentQuantity);
                Debug.Log($"Семена {currentSelectedItem.cropType} добавлены в количестве {currentQuantity}");
            }
            else
            {
                Debug.LogError("InventoryManager.Instance не найден! Семена не добавлены");
            }

                quantityPanel.SetActive(false);
        }
        else 
        { 
            if (errorPanel != null) errorPanel.SetActive(true);
        }
    }

    private int GetPlayerMoney()
    {
        return globalMoneyDisplay != null ? globalMoneyDisplay.GetMoney() : 0;
    }

    private void CloseQuantityPanel()
    {
        if (quantityPanel != null) quantityPanel.SetActive(false);
    }
    private void EnableMovementScripts(bool enable)
    {
        if (mattock != null) mattock.enabled = enable;
        if (wateringCan != null) wateringCan.enabled = enable;
        if (inventoryController != null) inventoryController.enabled = enable;
    }
    public void CloseShop()
    {
        IsShopOpen = false;

        EnableMovementScripts(true);
        if (Player.Instance != null) Player.Instance.SetMovementBlocked(false);

        if (shopWindow != null) shopWindow.SetActive(false);
        if (quantityPanel != null) quantityPanel.SetActive(false);
        if (errorPanel != null) errorPanel.SetActive(false);

        DialogueManager dm = FindObjectOfType<DialogueManager>();
        if (dm != null) dm.EndDialogue();
    }

  
}
