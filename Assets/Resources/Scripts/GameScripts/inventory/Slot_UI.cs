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

        eventData.pointerDrag = gameObject;
        originalPosition = itemIcon.transform.position;
        canvasGroup.alpha = 0.8f; // Робимо напівпрозорим
        canvasGroup.blocksRaycasts = false; // Дозволяємо "бачити" слоти під іконкою

        Canvas rootCanvas = inventoryUI.GetComponentInParent<Canvas>();
        while (rootCanvas.transform.parent != null &&
               rootCanvas.transform.parent.GetComponent<Canvas>() != null)
            rootCanvas = rootCanvas.transform.parent.GetComponent<Canvas>();

        itemIcon.transform.SetParent(rootCanvas.transform); 
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

        itemIcon.transform.SetParent(this.transform);
        itemIcon.transform.position = originalPosition;
        itemIcon.transform.SetAsFirstSibling();

        GameObject dropObject = eventData.pointerCurrentRaycast.gameObject;

        Slot_UI targetSlot = null;
        if (dropObject != null)
        {
            targetSlot = dropObject.GetComponent<Slot_UI>();
            if (targetSlot == null) targetSlot = dropObject.GetComponentInParent<Slot_UI>();
        }

        // Відпустили на іншому слоті: обмін
        if (targetSlot != null)
        {
            if (targetSlot != this) inventoryUI.SwapSlots(this, targetSlot);
            return;
        }

        // Відпустили поза панеллю інвентаря: викидаємо весь стак
        RectTransform panelRect = inventoryUI.inventoryPanel.GetComponent<RectTransform>();
        bool overPanel = inventoryUI.inventoryPanel.activeSelf &&
                         RectTransformUtility.RectangleContainsScreenPoint(panelRect, eventData.position);

        if (!overPanel && slotCollectible != CollectableType.NONE)
        {
            int id = inventoryUI.slots.IndexOf(this);
            if (id == -1) id = inventoryUI.slotsMini.IndexOf(this);
            if (id != -1) inventoryUI.RemoveStack(id);
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
