using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization.Settings; 

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

    // Підписатись на зміну мови
    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }
    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        StartCoroutine(RefreshDescription()); // оновити текст при зміні мови
    }

    IEnumerator InitializeWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Collectable item = ItemManager.Instance.GetItemByType(type);
        buyPrice = item.priceToBuy;
        priceText.text = buyPrice.ToString() + "$";
        itemIcon.sprite = item.icon;
        StartCoroutine(RefreshDescription());
    }

    IEnumerator RefreshDescription()
    {
        Collectable item = ItemManager.Instance.GetItemByType(type);

            var op = LocalizationSettings.StringDatabase
                .GetLocalizedStringAsync("Shop", item.localizationKey);
            yield return op;
            descriptionText.text = op.Result;
    }

    public void Buy()
    {
        Player pl = FindFirstObjectByType<Player>();
        if ((pl.GetCoins() - buyPrice) >= 0)
        {
            pl.inventory.Add(ItemManager.Instance.GetItemByType(type));
            pl.AddMoney(-buyPrice);
            FindAnyObjectByType<Inventory_UI>().Refresh();
        }
    }
}