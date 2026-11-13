using System.Collections;
using UnityEngine;

//Ресурс який можна добувати, з нього випадають collectible
public class Resource : MonoBehaviour, IInteractable
{
    [Header("Interact prompt (лише для shouldBeDestroyed = false)")]
    public GameObject interactPrompt; //ЮІ "Натисніть Е"
    public float interactRadius = 1.5f;

    public ResourceType type;    
    public Sprite[] sprites;
    [Header("Drop settings")]
    public DropEntry[] drops;

    public CollectableType[] requiredTools;

    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;
    private float cooldownTime = 0.4f;
    private float lastMineTime;

    public bool shouldBeDestroyed;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMineTime = -cooldownTime;

        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    private bool grown;

    void OnEnable() { InteractionManager.All.Add(this); }
    void OnDisable() { InteractionManager.All.Remove(this); SetPrompt(false); }

    void Update()
    {
        if (shouldBeDestroyed || spriteRenderer == null) return;
        grown = spriteRenderer.sprite == sprites[0]
                && Time.time > lastMineTime + cooldownTime + 0.1f;
        if (grown) currentSpriteIndex = 0;
    }

    public Vector2 InteractPoint => transform.position;
    public float InteractRadius => interactRadius;
    public bool CanInteract => !shouldBeDestroyed && grown;

    public void SetPrompt(bool visible)
    {
        if (interactPrompt != null) interactPrompt.SetActive(visible);
    }

    public void Interact()
    {
        ChangeResourceSprite(false);
        if (audioSource != null) audioSource.Play();
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

        if (drops == null) yield break;

        Transform collectablesParent = GameObject.Find("Collectables")?.transform;

        foreach (DropEntry entry in drops)
        {
            // кожен предмет кидає свій шанс окремо
            if (Random.Range(0f, 100f) > entry.chance) continue;

            Collectable itemPrefab = ItemManager.Instance.GetItemByType(entry.item);
            if (itemPrefab == null) continue;

            int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1.5f, -1.2f), 0f);
                Instantiate(itemPrefab, transform.position + spawnOffset, Quaternion.identity, collectablesParent);
            }
        }
    }
}
public enum ResourceType { Stone, Copper, Wood, Blueberry, Bush, Tree, Iron, Coal, Strawberry, Apple }
[System.Serializable]
public class DropEntry
{
    public CollectableType item;
    public int minAmount = 1;
    public int maxAmount = 1;
    [Range(0f, 100f)] public float chance = 100f; // шанс, %
}

