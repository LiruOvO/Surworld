using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopSlot_UI : MonoBehaviour, IDropHandler
{
    public Image itemIcon;
    public TextMeshProUGUI priceText;
    public TMP_InputField quantityInput; // ← змінили з Text на InputField
    public GameObject cancelCircle;
    public Button btnPlus;
    public Button btnMinus;

    private Inventory_UI inventoryUI;
    private Slot_UI selectedSourceSlot;
    private int quantity = 1;
    private int maxQuantity = 1;
    private Collectable currentItem;

    void Awake()
    {
        inventoryUI = Object.FindFirstObjectByType<Inventory_UI>();
    }

    void Start()
    {
        ClearSlot();

        btnPlus.onClick.AddListener(IncreaseQuantity);
        btnMinus.onClick.AddListener(DecreaseQuantity);

        // Ввід вручну
        quantityInput.onEndEdit.AddListener(OnQuantityEdited);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Slot_UI sourceSlot = eventData.pointerDrag.GetComponent<Slot_UI>();
            if (sourceSlot != null && sourceSlot.slotCollectible != CollectableType.NONE)
                SelectItemForSale(sourceSlot);
        }
    }

    void SelectItemForSale(Slot_UI sourceUI)
    {
        selectedSourceSlot = sourceUI;
        int slotIndex = inventoryUI.slots.IndexOf(sourceUI);
        if (slotIndex == -1) slotIndex = inventoryUI.slotsMini.IndexOf(sourceUI);

        if (slotIndex != -1)
        {
            Inventory.Slot inventorySlot = inventoryUI.player.inventory.slots[slotIndex];
            currentItem = ItemManager.Instance.GetItemByType(inventorySlot.type);
            maxQuantity = inventorySlot.count;

            if (currentItem != null)
            {
                Sprite iconFromManager = currentItem.GetComponent<SpriteRenderer>().sprite;
                itemIcon.sprite = iconFromManager;
                itemIcon.color = Color.white;

                quantity = 1;
                UpdateUI();
            }
        }

        if (cancelCircle != null) cancelCircle.SetActive(true);
        btnPlus.gameObject.SetActive(true);
        btnMinus.gameObject.SetActive(true);
        quantityInput.gameObject.SetActive(true);
    }

    void IncreaseQuantity()
    {
        if (quantity < maxQuantity) quantity++;
        UpdateUI();
    }

    void DecreaseQuantity()
    {
        if (quantity > 1) quantity--;
        UpdateUI();
    }

    void OnQuantityEdited(string value)
    {
        if (int.TryParse(value, out int parsed))
            quantity = Mathf.Clamp(parsed, 1, maxQuantity);
        UpdateUI();
    }

    void UpdateUI()
    {
        quantityInput.text = quantity.ToString();
        if (currentItem != null)
            priceText.text = $"{currentItem.priceToSell * quantity} $";
    }

    public void SellCurrentItem()
    {
        if (selectedSourceSlot != null)
        {
            int slotIndex = inventoryUI.slots.IndexOf(selectedSourceSlot);
            if (slotIndex == -1) slotIndex = inventoryUI.slotsMini.IndexOf(selectedSourceSlot);

            if (slotIndex != -1)
            {
                for (int i = 0; i < quantity; i++)
                {
                    inventoryUI.player.AddMoney(currentItem.priceToSell);
                    inventoryUI.player.inventory.Remove(slotIndex);
                }
                inventoryUI.Refresh();
            }

            selectedSourceSlot = null;
            ClearSlot();
        }
    }

    public void CancelSale()
    {
        selectedSourceSlot = null;
        ClearSlot();
    }

    public void ClearSlot()
    {
        selectedSourceSlot = null;
        currentItem = null;
        quantity = 1;
        maxQuantity = 1;

        if (itemIcon != null) { itemIcon.sprite = null; itemIcon.color = new Color(1, 1, 1, 0); }
        if (priceText != null) priceText.text = "";
        if (quantityInput != null) quantityInput.text = "";
        if (cancelCircle != null) cancelCircle.SetActive(false);
        if (btnPlus != null) btnPlus.gameObject.SetActive(false);
        if (btnMinus != null) btnMinus.gameObject.SetActive(false);
        if (quantityInput != null) quantityInput.gameObject.SetActive(false);
    }
}