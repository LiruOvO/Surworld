using System.Collections;
using System.Security.Cryptography;
using UnityEngine;


//Скрипт для перегенерації ресурсів на карті
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
            foreach (GameObject res in resSP)
            {
                if (res.transform.childCount == 0)
                {
                    StartCoroutine(SpawnRes(res));
                }
                else if (res.transform.childCount == 1 && res.transform.GetChild(0).GetComponent<Resource>().shouldBeDestroyed == false)
                {
                    StartCoroutine(ChangeSprite(res));
                }
            }            

            yield return new WaitForSeconds(20);
        }
    }
    private IEnumerator SpawnRes(GameObject res)
    {
        float delay = Random.Range(1f, 5f);
        yield return new WaitForSeconds(delay);

        Resource itemPrefab = ItemManager.Instance.GetItemByType(resourceType);
        Instantiate(itemPrefab, res.transform.position, Quaternion.identity, res.transform);
    }

    private IEnumerator ChangeSprite(GameObject spawnPoint)
    {
        float delay = Random.Range(5f, 10f);
        yield return new WaitForSeconds(delay);

        Transform currentRes = spawnPoint.transform.GetChild(0);
        SpriteRenderer sr = currentRes.GetComponent<SpriteRenderer>();
        Resource itemPrefab = ItemManager.Instance.GetItemByType(resourceType);
        sr.sprite = itemPrefab.sprites[0];
    }

    }
