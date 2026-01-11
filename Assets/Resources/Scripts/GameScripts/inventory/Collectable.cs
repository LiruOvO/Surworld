using UnityEngine;

//Скрипт що чіпляється до предметів які можна збирати в інвентар
public abstract class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite[] spritesForAnimation;
    public Sprite icon;
    private Inventory_UI inventoryScript;
    public int maxAllowed = 100;

    public bool isConsumable;


    private void Start()
    {
        inventoryScript = FindFirstObjectByType<Inventory_UI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>(); //Якщо це гравець то матиме компонент плеєр, якщо ж ні, то значення була нал
        if (player != null)
        {
            //Предмет зникає тільки якщо може підібратись в інвентар
            bool addedSuccessfully = player.inventory.Add(this);
            if (addedSuccessfully)
            {
                inventoryScript.Refresh();
                Destroy(this.gameObject);
            }
        }        
    }
    public abstract void Use(Player player);
}

public enum CollectableType
{
    NONE,
    WOODEN_PICKAXE, STONE_PICKAXE, COPPER_PICKAXE, 
    STONE, COPPER, BLUEBERRY
}




