using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Inventory;
using UnityEngine.EventSystems;

//Відображення слотів в інвенторі
public class Slot_UI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    private Image uiImage;
    private Inventory_UI inventoryUI;

    public CollectableType slotCollectible;//зберіягає який предмет в слоті

    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    void Awake()
    {
        uiImage = GetComponent<Image>();
        inventoryUI = GetComponentInParent<Inventory_UI>();

        // Додай CanvasGroup на префаб слота, щоб іконка була прозорою при перетягуванні
        canvasGroup = itemIcon.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = itemIcon.gameObject.AddComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryUI != null) inventoryUI.HandleSlotSelection(this);
    }

    // --- ЛОГІКА ПЕРЕТЯГУВАННЯ ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotCollectible == CollectableType.NONE) return;

        originalPosition = itemIcon.transform.position;
        canvasGroup.alpha = 0.8f; // Робимо напівпрозорим
        canvasGroup.blocksRaycasts = false; // Дозволяємо "бачити" слоти під іконкою

        itemIcon.transform.SetParent(inventoryUI.transform);
        itemIcon.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slotCollectible == CollectableType.NONE) return;

        // Іконка слідує за мишкою
        itemIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 3. Повертаємо іконку назад у її рідний слот
        itemIcon.transform.SetParent(this.transform);
        itemIcon.transform.position = originalPosition;

        // Встановлюємо порядок у самому слоті (щоб іконка була під текстом кількості, якщо треба)
        itemIcon.transform.SetAsFirstSibling();

        // Логіка обміну (та сама, що вже була)
        GameObject dropObject = eventData.pointerCurrentRaycast.gameObject;
        if (dropObject != null)
        {
            Slot_UI targetSlot = dropObject.GetComponent<Slot_UI>();
            if (targetSlot == null) targetSlot = dropObject.GetComponentInParent<Slot_UI>();

            if (targetSlot != null && targetSlot != this)
            {
                inventoryUI.SwapSlots(this, targetSlot);
            }
        }
    }
    public void ResetColor()
    {
        uiImage = GetComponent<Image>();
        uiImage.color = Color.white;
    }
    public void SelectColor()
    {
        uiImage = GetComponent<Image>();
        uiImage.color = Color.black;   
    }



    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null)
        {
            slotCollectible = slot.type;
            itemIcon.sprite =  slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()
    {
        slotCollectible = CollectableType.NONE;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1,1,1,0);
        quantityText.text = "";
    }

   
}
