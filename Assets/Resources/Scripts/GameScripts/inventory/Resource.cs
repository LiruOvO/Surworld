using System.Collections;
using UnityEngine;

//Ресурс який можна добувати, з нього випадають collectible
public class Resource : MonoBehaviour
{
    public ResourceType type;    
    public Sprite[] sprites;
    public CollectableType itemToDropType;

    public CollectableType[] requiredTools;

    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;
    private float cooldownTime = 0.4f;
    private float lastMineTime;

    public bool shouldBeDestroyed;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMineTime = -cooldownTime;
    }
    public bool CanInteractWith(CollectableType toolType)
    {
        // Перевіряє, чи присутній тип інструменту (toolType) у списку requiredTools
        if (requiredTools == null) return false;
        return System.Array.Exists(requiredTools, element => element == toolType);
    }

    public void ChangeResourceSprite(bool destroy)
    {
        if (Time.time < lastMineTime + cooldownTime)
        {
            return;
        }
        lastMineTime = Time.time;

        if (destroy)
        {
            currentSpriteIndex++;
            if (currentSpriteIndex < sprites.Length)
            {
                StartCoroutine(ChangeSpriteWithDelay(cooldownTime));
            }
            else
            {
                StartCoroutine(DropItem(cooldownTime - 0.1f));
                Destroy(gameObject, cooldownTime);
            }
        }else if (!destroy)
        {
            if(currentSpriteIndex == 0)
            {
                currentSpriteIndex++;
                StartCoroutine(ChangeSpriteWithDelay(cooldownTime));
                StartCoroutine(DropItem(cooldownTime - 0.1f));
            }else if (spriteRenderer.sprite == sprites[0])
            {
                currentSpriteIndex = 0;
            }            
        }
            
    }
    private IEnumerator ChangeSpriteWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex]; 
        }
    }

    private IEnumerator DropItem(float delay)
    {
        yield return new WaitForSeconds(delay);
        Collectable itemPrefab = ItemManager.Instance.GetItemByType(itemToDropType);

        Vector3 spawnLocation = transform.position;
        Vector3 spawnOffset = new Vector3(Random.Range(-0.3f, 0f), Random.Range(-1.3f, -1.5f), 0f);

        Transform collectablesParent = GameObject.Find("Collectables")?.transform;
        Instantiate(itemPrefab, spawnLocation + spawnOffset, Quaternion.identity, collectablesParent);
    }
}
public enum ResourceType { Stone, Copper, Wood, Blueberry, Bush, Tree, Iron, Coal, Strawberry }

