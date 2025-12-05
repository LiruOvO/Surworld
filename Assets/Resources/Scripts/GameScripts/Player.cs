using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    private Transform collectablesParent;

    private void Awake()
    {
        inventory = new Inventory(10);//10 - це кть слотів в інвенторі
    }
    private void Start()
    {
        GameObject parentObject = GameObject.Find("Collectables");
        if (parentObject != null)
        {
            collectablesParent = parentObject.transform;
        }
    }

    //Викидання предметів з інвентарю гравця
    public void DropItem(Collectable item)
    {
        Vector3 spawnLocation = transform.position;
        Vector3 spawnOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-1.3f,-1.5f), 0f);
        Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity, collectablesParent);
    }
}
