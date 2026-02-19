using Mono.Cecil;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPref;
    public GameObject[] resSP;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }


    private IEnumerator SpawnEnemy()
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

            yield return new WaitForSeconds(20);
        }
    }

    private IEnumerator SpawnRes(GameObject res)
    {
        float delay = Random.Range(1f, 5f);
        yield return new WaitForSeconds(delay);

        Instantiate(enemyPref, res.transform.position, Quaternion.identity, res.transform);
    }
}
