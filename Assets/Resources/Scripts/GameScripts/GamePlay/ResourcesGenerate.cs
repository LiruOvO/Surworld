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
            foreach (GameObject res in resSP)
            {
                if (res.transform.childCount == 0)
                {
                    StartCoroutine(SpawnRes(res));
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

}
