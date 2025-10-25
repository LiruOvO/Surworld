using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>(); //Якщо це гравець то матиме компонент плеєр, якщо ж ні, то значення була нал

        if (player) {
            player.inventory.Add(this);
            Destroy(this.gameObject); 
        }
    }
}

public enum CollectableType
{
    NONE, TOMATO
}
