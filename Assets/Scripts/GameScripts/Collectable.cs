using UnityEngine;

//Предмети які можна збирати в інвентар
public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;
    private Inventory_UI inventoryScript;

    private void Start()
    {
        inventoryScript = FindFirstObjectByType<Inventory_UI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>(); //Якщо це гравець то матиме компонент плеєр, якщо ж ні, то значення була нал

        //Предмет зникає тільки якщо може підібратись в інвентар
        bool addedSuccessfully = player.inventory.Add(this);
        if (addedSuccessfully)
        {
            inventoryScript.Setup();
            Destroy(this.gameObject);
        }
    }
}

public enum CollectableType
{
    NONE, TOMATO
}
