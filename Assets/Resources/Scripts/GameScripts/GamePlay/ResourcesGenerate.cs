using System.Collections;
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
            Resource itemPrefab = ItemManager.Instance.GetItemByType(resourceType);

            foreach (GameObject res in resSP)
            {
                if (res.transform.childCount == 0)
                {
                    Instantiate(itemPrefab, res.transform.position, Quaternion.identity, res.transform);
                }
            }

            yield return new WaitForSeconds(10);
        }
    }

}
