using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Animation")]
    public Animator handsAnimator;

    [Header("Hotbar")]
    public int hotbarSize = 7;
    public SpriteRenderer[] slotRenderers;
    public Color activeSlotColor = Color.yellow;
    public Color normalSlotColor = Color.white;

    [Header("Inventory")]
    public GameObject fullInventoryUI;
    public Inventory mainInventory;
    public DataBase database;
    public Transform player;

    [Header("Drag & Drop")]
    public GameObject dragItemPrefab;
    public float dragOffset = 1f;

    private int currentSlot;
    private bool isInventoryOpen;
    private Vector3 lastPlayerPosition;
    private GameObject currentDragItem;
    private int dragOriginSlot = -1;
    
    private PauseButtonPosition pauseButton;
    private float inventoryOpenTime;

    public const int TOOL_SLOTS = 2; // Первые два слота для инструментов

    public static InventoryController Instance { get; private set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        SelectSlot(0);
        InitializeHotbar();
        UpdateHotbarVisuals();

        if (fullInventoryUI != null)
            fullInventoryUI.SetActive(false);
        
        isInventoryOpen = false;

        if (player != null)
            lastPlayerPosition = player.position;

        pauseButton = FindObjectOfType<PauseButtonPosition>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (pauseButton != null && pauseButton.IsMenuOpen()) return;

        HandlePlayerMovement();
        HandleInput();
        HandleDragAndDrop();
    }

    // ======================== PUBLIC API ========================

    public int GetSelectedSlot() => currentSlot;

    public Item GetSelectedItem() => GetItemInSlot(currentSlot);

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null || item.id == 0 || quantity <= 0)
            return false;

        for (int i = TOOL_SLOTS; i < mainInventory.items.Count; i++)
        {
            if (mainInventory.items[i].id == 0)
            {
                mainInventory.items[i].id = item.id;
                mainInventory.items[i].count = quantity;
                UpdateAllVisuals();
                return true;
            }

            if (mainInventory.items[i].id == item.id && mainInventory.items[i].count < item.maxStack)
            {
                mainInventory.items[i].count += quantity;
                UpdateAllVisuals();
                return true;
            }
        }

        Debug.LogWarning("Нет свободных слотов в инвентаре!");
        return false;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        for (int i = 0; i < mainInventory.items.Count; i++)
        {
            if (mainInventory.items[i].id != item.id) continue;

            int removeAmount = Mathf.Min(quantity, mainInventory.items[i].count);
            mainInventory.items[i].count -= removeAmount;

            if (mainInventory.items[i].count <= 0)
            {
                mainInventory.items[i].id = 0;
                mainInventory.items[i].count = 0;
            }

            UpdateAllVisuals();
            return true;
        }

        return false;
    }

    public bool IsToolSlot(int slotIndex) => slotIndex < TOOL_SLOTS;

    // ======================== PRIVATE ========================

    private void InitializeHotbar()
    {
        if (slotRenderers != null && slotRenderers.Length > 0) return;

        slotRenderers = new SpriteRenderer[hotbarSize + 1];
        for (int i = 0; i <= hotbarSize; i++)
        {
            string slotName = i < hotbarSize ? $"Slot_{i + 1}" : "InventoryButton";
            Transform slot = transform.Find(slotName);
            if (slot != null)
            {
                slotRenderers[i] = slot.GetComponent<SpriteRenderer>();
                
                if (i < hotbarSize && slot.GetComponent<BoxCollider2D>() == null)
                {
                    var collider = slot.gameObject.AddComponent<BoxCollider2D>();
                    collider.size = new Vector2(1, 1);
                }
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
            return;
        }

        if (!isInventoryOpen)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                SelectSlot((currentSlot + (scroll > 0 ? -1 : 1) + hotbarSize) % hotbarSize);

            for (int i = 0; i < hotbarSize; i++)
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    SelectSlot(i);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HandleItemUse(mousePos);
            HandleMouseClick();
        }
    }

    private void HandlePlayerMovement()
    {
        if (!isInventoryOpen || player == null) return;

        if (Time.time - inventoryOpenTime < 0.3f)
        {
            lastPlayerPosition = player.position;
            return;
        }

        if (Vector3.Distance(player.position, lastPlayerPosition) > 0.1f)
            CloseInventory();

        lastPlayerPosition = player.position;
    }

    private void HandleMouseClick()
    {
        if (currentDragItem != null) return;

        RaycastHit2D hit = GetRaycastHitAtMouse();
        if (hit.collider == null) return;

        for (int i = 0; i < slotRenderers.Length; i++)
        {
            if (slotRenderers[i] == null || hit.collider.gameObject != slotRenderers[i].gameObject)
                continue;

            if (i == hotbarSize)
                ToggleInventory();
            else if (i < hotbarSize)
                SelectSlot(i);
            break;
        }
    }

    private void HandleDragAndDrop()
    {
        if (Input.GetMouseButtonDown(0) && !isInventoryOpen)
            TryStartDrag();

        if (Input.GetMouseButton(0) && currentDragItem != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            currentDragItem.transform.position = mousePos + new Vector3(0, dragOffset, 0);
        }

        if (Input.GetMouseButtonUp(0) && currentDragItem != null)
            EndDrag();
    }

    private void TryStartDrag()
    {
        RaycastHit2D hit = GetRaycastHitAtMouse();
        if (hit.collider == null) return;

        for (int i = 0; i < hotbarSize; i++)
        {
            if (slotRenderers[i] == null || hit.collider.gameObject != slotRenderers[i].gameObject)
                continue;

            if (IsToolSlot(i)) return; // Инструменты нельзя перетаскивать

            Item item = GetItemInSlot(i);
            if (item == null || item.id == 0) return;

            dragOriginSlot = i;
            currentDragItem = Instantiate(dragItemPrefab);
            currentDragItem.GetComponent<SpriteRenderer>().sprite = item.img;
            return;
        }
    }

    private void EndDrag()
    {
        RaycastHit2D hit = GetRaycastHitAtMouse();
        bool droppedOnSlot = false;

        if (hit.collider != null)
        {
            for (int i = 0; i < hotbarSize; i++)
            {
                if (slotRenderers[i] == null || hit.collider.gameObject != slotRenderers[i].gameObject)
                    continue;

                if (IsToolSlot(i)) break; // Нельзя бросить в слот инструмента

                SwapSlots(dragOriginSlot, i);
                droppedOnSlot = true;
                break;
            }
        }

        if (!droppedOnSlot && mainInventory != null && isInventoryOpen)
            MoveItemToMainInventory(dragOriginSlot);

        Destroy(currentDragItem);
        currentDragItem = null;
        dragOriginSlot = -1;
    }

    private void MoveItemToMainInventory(int hotbarSlotIndex)
    {
        if (mainInventory == null || hotbarSlotIndex < 0 || hotbarSlotIndex >= hotbarSize)
            return;

        Item item = GetItemInSlot(hotbarSlotIndex);
        if (item == null || item.id == 0) return;

        if (!mainInventory.AddItemToFirstFreeSlot(item, 1)) return;

        mainInventory.items[hotbarSlotIndex].count--;
        if (mainInventory.items[hotbarSlotIndex].count <= 0)
            mainInventory.items[hotbarSlotIndex].id = 0;

        UpdateAllVisuals();
    }

    private void SwapSlots(int fromSlot, int toSlot)
    {
        if (mainInventory == null) return;

        (mainInventory.items[fromSlot], mainInventory.items[toSlot]) =
            (mainInventory.items[toSlot], mainInventory.items[fromSlot]);

        UpdateAllVisuals();
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= hotbarSize) return;
        currentSlot = index;
        UpdateHotbarVisuals();
    }

    private void UpdateHotbarVisuals()
    {
        if (slotRenderers == null) return;

        for (int i = 0; i < slotRenderers.Length; i++)
        {
            if (slotRenderers[i] == null) continue;

            if (i < hotbarSize)
            {
                slotRenderers[i].color = (i == currentSlot) ? activeSlotColor : normalSlotColor;

                if (!IsToolSlot(i))
                {
                    Item item = GetItemInSlot(i);
                    slotRenderers[i].sprite = item?.img;
                    slotRenderers[i].enabled = item != null;
                }
                else
                {
                    slotRenderers[i].enabled = true;
                }
            }
            else
            {
                slotRenderers[i].color = normalSlotColor;
            }
        }
    }

    private void UpdateAllVisuals()
    {
        UpdateHotbarVisuals();
        if (isInventoryOpen)
            mainInventory?.UpdateInventory();
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (fullInventoryUI != null)
        {
            fullInventoryUI.SetActive(isInventoryOpen);
            if (isInventoryOpen)
            {
                inventoryOpenTime = Time.time;
                lastPlayerPosition = player.position;
                mainInventory?.UpdateInventory();
            }
        }
    }

    private void CloseInventory()
    {
        if (!isInventoryOpen) return;

        isInventoryOpen = false;
        fullInventoryUI?.SetActive(false);
        Time.timeScale = 1f;
        UpdateHotbarVisuals();
    }

    private RaycastHit2D GetRaycastHitAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Hotbar");
        return Physics2D.Raycast(mousePos, Vector2.zero, 10f, layerMask);
    }

    private Item GetItemInSlot(int slotIndex)
    {
        if (mainInventory == null || slotIndex >= mainInventory.items.Count)
            return null;

        return database.GetItemById(mainInventory.items[slotIndex].id);
    }

    private void HandleItemUse(Vector2 worldPosition)
    {
        Item selectedItem = GetSelectedItem();
        if (selectedItem == null) return;

        if (selectedItem.IsSeed() && CropsManager.Instance.TryPlantSeed(selectedItem, worldPosition))
            RemoveItem(selectedItem, 1);
    }
}