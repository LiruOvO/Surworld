using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;
    public CraftingRecipe[] recipes;
    private Inventory playerInventory;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerInventory = FindFirstObjectByType<Player>().inventory;
    }

    public bool TryCraft(CraftingRecipe recipe)
    {
        playerInventory = FindFirstObjectByType<Player>().inventory;

        // Перевіряємо інгредієнти
        foreach (var ingredient in recipe.ingredients)
        {
            if (playerInventory.GetAmount(ingredient.type) < ingredient.amount)
            {
                Debug.Log("Недостатньо: " + ingredient.type);
                return false;
            }
        }

        // ✅ Перевіряємо чи є місце для результату
        Collectable resultItem = ItemManager.Instance.GetItemByType(recipe.result);
        bool hasSpace = false;
        foreach (var slot in playerInventory.slots)
        {
            if (slot.type == recipe.result && slot.count < resultItem.maxAllowed)
            {
                hasSpace = true;
                break;
            }
            if (slot.type == CollectableType.NONE)
            {
                hasSpace = true;
                break;
            }
        }

        if (!hasSpace)
        {
            Debug.Log("Немає місця в інвентарі!");
            return false;
        }

        // Забираємо інгредієнти
        foreach (var ingredient in recipe.ingredients)
        {
            playerInventory.Remove(ingredient.type, ingredient.amount);
        }

        // Додаємо результат
        for (int i = 0; i < recipe.resultAmount; i++)
        {
            playerInventory.Add(resultItem);
        }

        FindFirstObjectByType<Inventory_UI>().Refresh();
        FindFirstObjectByType<Crafting_UI>().Refresh();
        return true;
    }
}