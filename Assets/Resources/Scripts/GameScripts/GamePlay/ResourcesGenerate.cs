using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class ResourcesGenerate : MonoBehaviour
{
    public ResourceType resourceType;
    public GameObject[] resSP;

    private void Start()
    {
        StartCoroutine(SpawnResource());
    }

    private IEnumerator SpawnResource()
    {
        while (true)
        {
            Resource isDestroyed = ItemManager.Instance.GetItemByType(resourceType);
            if (isDestroyed.shouldBeDestroyed)
            {
                foreach (GameObject res in resSP)
                {
                    if (res.transform.childCount == 0)
                    {
                        StartCoroutine(SpawnRes(res));
                    }
                }
            }
            else
            {
                foreach (GameObject res in resSP)
                {
                    if (res.transform.childCount == 0)
                    {
                        StartCoroutine(ChangeSprite(isDestroyed.sprites));
                    }
                }
            }
            

            yield return new WaitForSeconds(10);
        }
    }
    private IEnumerator SpawnRes(GameObject res)
    {
        float delay = Random.Range(1f, 5f);
        yield return new WaitForSeconds(delay);

        Resource itemPrefab = ItemManager.Instance.GetItemByType(resourceType);
        Instantiate(itemPrefab, res.transform.position, Quaternion.identity, res.transform);
    }

    private IEnumerator ChangeSprite(Sprite[] sprites)
    {
        float delay = Random.Range(1f, 5f);
        yield return new WaitForSeconds(delay);
        Resource itemPrefab = ItemManager.Instance.GetItemByType(resourceType);
        itemPrefab.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    }
