using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(10);//10 - це кть слотів в інвенторі
    }
}
