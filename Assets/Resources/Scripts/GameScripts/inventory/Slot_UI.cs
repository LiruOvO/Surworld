using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Inventory;
using UnityEngine.EventSystems;

//Відображення слотів в інвенторі
public class Slot_UI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    private Image uiImage;
    private Inventory_UI inventoryUI;

    public Collectable slotItemData; //зберіягає який предмет в слоті

    void Awake()
    {
        uiImage = GetComponent<Image>();
        inventoryUI = GetComponentInParent<Inventory_UI>();
    }
    //Виділення слота при натисканні на нього, та прибирання виділення при повторному натисканні
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryUI != null)
        {
            inventoryUI.HandleSlotSelection(this);
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
            slotItemData = slot.itemData;
            itemIcon.sprite = slot.itemData.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()
    {
        slotItemData = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1,1,1,0);
        quantityText.text = "";
    }

   
}
