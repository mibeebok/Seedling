using UnityEngine;
using UnityEngine.UI;

public class SellItemsSlotUI : MonoBehaviour
{
    public Image icon;
    public Button sellButton;
    public Button minusButton;
    public Button plusButton;
    public Text quantityText;

    private Item currentItem;
    private int maxCount;
    private int currentQuantity = 0;
    private SellHarvestUI parentUI;

    public void Initialize(Item item, int availableCount, SellHarvestUI parent)
    {
        currentItem = item;
        maxCount = availableCount;
        parentUI = parent;

        if (icon != null) icon.sprite = item.img;
        if (sellButton != null)
        {
            Text buttonText = sellButton.GetComponentInChildren<Text>();
            if (buttonText != null) buttonText.text = $"Продать {GetRussianName(item)}";
            sellButton.onClick.AddListener(OnSellClicked);
        }

        if (minusButton != null) minusButton.onClick.AddListener(OnMinus);
        if (plusButton != null) plusButton.onClick.AddListener(OnPlus);

        UpdateQuantityDisplay();

    }

    public int GetItemId() => currentItem.id;

    public void UpdateCount(int newMaxCount)
    {
        maxCount = newMaxCount;
        if (currentQuantity > maxCount) currentQuantity = maxCount;
        UpdateQuantityDisplay();

        if (sellButton != null) sellButton.interactable = maxCount > 0;
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
   void OnPlus()
    {
        if (currentQuantity < maxCount)
            currentQuantity++;
        UpdateQuantityDisplay();
    }

    void OnMinus()
    {
        if (currentQuantity > 0)
            currentQuantity--;
        UpdateQuantityDisplay();
    }

    void OnSellClicked()
    {
        if (currentQuantity > 0)
            parentUI.SellVegetable(currentItem, currentQuantity);
        else
            parentUI.ShowConfirmMessage("Выберите количество для продажи.");
    }

    void UpdateQuantityDisplay()
    {
        if (quantityText != null)
            quantityText.text = currentQuantity.ToString();
    }
}
