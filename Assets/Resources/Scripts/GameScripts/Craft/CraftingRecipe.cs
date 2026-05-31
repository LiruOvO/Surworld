using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public CollectableType type;
        public int amount;
    }

    public Ingredient[] ingredients;
    public CollectableType result;
    public int resultAmount = 1;
}