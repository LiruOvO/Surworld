using UnityEngine;

public class Crafting_UI : MonoBehaviour
{
    public GameObject craftingPanel;
    public Transform recipesContent; // Content всередині ScrollView
    public GameObject recipeRowPrefab;
    public Player player;

    private bool isOpen = false;

    void Start()
    {
        craftingPanel.SetActive(false);
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
        craftingPanel.SetActive(isOpen);
        if (isOpen) Refresh();
    }

    public void Refresh()
    {
        // Очищаємо старі рядки
        foreach (Transform child in recipesContent)
        {
            Destroy(child.gameObject);
        }

        // Створюємо рядок для кожного рецепту
        foreach (CraftingRecipe recipe in CraftingManager.Instance.recipes)
        {
            GameObject row = Instantiate(recipeRowPrefab, recipesContent);
            row.GetComponent<RecipeRow_UI>().Setup(recipe, player.inventory);
        }
    }

    public void Close()
    {
        isOpen = false;
        craftingPanel.SetActive(false);
    }
}