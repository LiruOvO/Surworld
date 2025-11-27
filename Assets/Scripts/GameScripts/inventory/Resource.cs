using System.Collections;
using UnityEngine;

//Ресурс який можна добувати, з нього випадають collectible
public class Resource : MonoBehaviour
{
    public enum ResourceType { Stone, Wood }
    public ResourceType type;
    public Sprite[] sprites;

    private int currentSpriteIndex = 0;
    private SpriteRenderer spriteRenderer;
    private float cooldownTime = 0.4f;
    private float lastMineTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMineTime = -cooldownTime;
    }
    public void ChangeResourceSprite()
    {
        if (Time.time < lastMineTime + cooldownTime)
             {
                return;
             }
        lastMineTime = Time.time;

        currentSpriteIndex++;
        if (currentSpriteIndex < sprites.Length)
        {
            StartCoroutine(ChangeSpriteWithDelay(cooldownTime));
        }
        else
        {
            Destroy(gameObject, cooldownTime);
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
}
