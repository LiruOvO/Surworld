using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopSlot_UI : MonoBehaviour, IDropHandler
{
    public Image itemIcon;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;

    private Inventory_UI inventoryUI;
    private Slot_UI selectedSourceSlot; // Запам'ятовуємо, звідки прийшов предмет

    public GameObject cancelCircle;
    void Awake()
    {
        inventoryUI = Object.FindFirstObjectByType<Inventory_UI>();
    }

    void Start()
    {
        ClearSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Slot_UI sourceSlot = eventData.pointerDrag.GetComponent<Slot_UI>();

            if (sourceSlot != null && sourceSlot.slotCollectible != CollectableType.NONE)
            {
                SelectItemForSale(sourceSlot);
            }
        }
    }

    void SelectItemForSale(Slot_UI sourceUI)
    {
        selectedSourceSlot = sourceUI; // Зберігаємо посилання на слот інвентарю

        int slotIndex = inventoryUI.slots.IndexOf(sourceUI);
        if (slotIndex == -1) slotIndex = inventoryUI.slotsMini.IndexOf(sourceUI);

        if (slotIndex != -1)
        {
            Inventory.Slot inventorySlot = inventoryUI.player.inventory.slots[slotIndex];
            Collectable itemData = ItemManager.Instance.GetItemByType(inventorySlot.type);

            if (itemData != null)
            {
                // Візуально відображаємо в магазині
                Sprite iconFromManager = itemData.GetComponent<SpriteRenderer>().sprite;
                itemIcon.sprite = iconFromManager;
                itemIcon.color = Color.white;

                quantityText.text = "1"; // Або inventorySlot.count.ToString()
                priceText.text = $"{itemData.priceToSell} $";
            }
        }
        if (cancelCircle != null) cancelCircle.SetActive(true);
    }

    // Цей метод потрібно повісити на кнопку "SELL" в Unity
    public void SellCurrentItem()
    {
        if (selectedSourceSlot != null)
        {
            // Викликаємо продаж через існуючу логіку інвентарю
            inventoryUI.SellItem(selectedSourceSlot);

            // Після продажу очищаємо слот магазину
            selectedSourceSlot = null;
            ClearSlot();
        }
    }
    // Цей метод вішаємо на кнопку-хрестик
    public void CancelSale()
    {
        // Просто забуваємо про посилання на слот інвентарю 
        // і очищаємо візуал. Предмет в інвентарі лишається недоторканим.
        selectedSourceSlot = null;
        ClearSlot();
    }

    public void ClearSlot()
    {
        selectedSourceSlot = null;
        if (itemIcon != null) { itemIcon.sprite = null; itemIcon.color = new Color(1, 1, 1, 0); }
        if (priceText != null) priceText.text = "";
        if (quantityText != null) quantityText.text = "";

        // Ховаємо хрестик, якщо в слоті порожньо
        if (cancelCircle != null) cancelCircle.SetActive(false);
    }
}