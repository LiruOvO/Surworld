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
    void Start()
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
        uiImage.color = Color.white;
    }
    public void SelectColor()
    {
        uiImage.color = Color.black;
    }



    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null)
        {
            itemIcon.sprite =  slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1,1,1,0);
        quantityText.text = "";
    }

   
}
