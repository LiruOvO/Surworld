using UnityEngine;
using UnityEngine.Tilemaps;


//Змінює порядок в шарах для об'єкту, коли заходить гравець
public class SortingSwitch : MonoBehaviour
{
    [SerializeField] private int playerInFrontOrder = 3;
    [SerializeField] private int playerBehindOrder = 15;

    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tilemapRenderer;

    private int originalSortingOrder;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemapRenderer = GetComponent<TilemapRenderer>();

        if (spriteRenderer != null)
        {
            originalSortingOrder = spriteRenderer.sortingOrder;
        }
        else if (tilemapRenderer != null)
        {
            originalSortingOrder = tilemapRenderer.sortingOrder;
        }
    }

    private void SetSortingOrder(int order)
    {        
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }        
        else if (tilemapRenderer != null)
        {
            tilemapRenderer.sortingOrder = order;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            string otherName = other.gameObject.name;

            if (otherName == "BottomTrigger")
            {
                SetSortingOrder(playerBehindOrder);
            }
            else if (otherName == "TopTrigger")
            {
                SetSortingOrder(playerInFrontOrder);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            string otherName = other.gameObject.name;

            if (otherName == "TopTrigger" || otherName == "BottomTrigger")
            {
                // Повертаємо об'єкт до оригінального (дефолтного) порядку сортування
                SetSortingOrder(originalSortingOrder);
            }
        }
    }
}

