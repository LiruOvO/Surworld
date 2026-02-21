using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class ShopItemToBuy_UI : MonoBehaviour
{
    public CollectableType type;
    public Image itemIcon;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI descriptionText;


    private int buyPrice;

    private void Start()
    {
        StartCoroutine(InitializeWithDelay());
    }
    public void Buy()
    {
        Player pl = FindFirstObjectByType<Player>();
        int plCoins = pl.GetCoins();
        if ((plCoins - buyPrice) >= 0)
        {
            pl.inventory.Add(ItemManager.Instance.GetItemByType(type));
            pl.AddMoney(-buyPrice);
            FindAnyObjectByType<Inventory_UI>().Refresh();
        }
    }
    IEnumerator InitializeWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        buyPrice = ItemManager.Instance.GetItemByType(type).priceToBuy;
        priceText.text = buyPrice.ToString() + "$";
        descriptionText.text = ItemManager.Instance.GetItemByType(type).description;
        Debug.Log("Менеджер висить на об'єкті: " + ItemManager.Instance.gameObject.name);
        itemIcon.sprite = ItemManager.Instance.GetItemByType(type).icon;
    }

        
}
