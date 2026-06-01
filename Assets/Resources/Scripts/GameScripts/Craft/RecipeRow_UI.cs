using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Скрипт для рядків із рецептами крафту

public class RecipeRow_UI : MonoBehaviour
{
    public Image[] ingredientIcons;
    public TextMeshProUGUI[] ingredientCounts;
    public Image resultIcon;
    public TextMeshProUGUI resultCount;
    public Button craftButton;
    public TextMeshProUGUI craftButtonText;

    private CraftingRecipe recipe;

    public GameObject plusSign; // перетягни PlusText сюди в Inspector

    public void Setup(CraftingRecipe recipe, Inventory playerInventory)
    {
        this.recipe = recipe;

        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (i < recipe.ingredients.Length)
            {
                Collectable item = ItemManager.Instance.GetItemByType(recipe.ingredients[i].type);
                ingredientIcons[i].sprite = item.icon;
                ingredientIcons[i].gameObject.SetActive(true);
                ingredientCounts[i].gameObject.SetActive(true);
                ingredientCounts[i].text = "x" + recipe.ingredients[i].amount;
            }
            else
            {
                ingredientIcons[i].gameObject.SetActive(false);
                ingredientCounts[i].gameObject.SetActive(false);
            }
        }

        // ✅ Ховаємо + якщо лише один інгредієнт
        if (plusSign != null)
            plusSign.SetActive(recipe.ingredients.Length > 1);

        // Результат
        Collectable resultItem = ItemManager.Instance.GetItemByType(recipe.result);
        resultIcon.sprite = resultItem.icon;
        resultCount.text = "x" + recipe.resultAmount;

        bool canCraft = CanCraft(playerInventory);
        craftButton.interactable = canCraft;
        craftButtonText.color = canCraft ? Color.white : Color.gray;

        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(() => OnCraftClicked());
    }

    private bool CanCraft(Inventory inventory)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (inventory.GetAmount(ingredient.type) < ingredient.amount)
                return false;
        }
        return true;
    }

    private void OnCraftClicked()
    {
        bool success = CraftingManager.Instance.TryCraft(recipe);
        Debug.Log("Крафт результат: " + success);
        FindFirstObjectByType<Crafting_UI>().Refresh();
    }
}