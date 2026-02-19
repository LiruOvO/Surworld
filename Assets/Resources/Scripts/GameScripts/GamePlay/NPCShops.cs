using UnityEngine;

public class NPCShops : MonoBehaviour
{
    public GameObject shopWindow;



    public void CloseShop()
    {
        shopWindow.SetActive(false);
    }

    public void OpenShop()
    {
        shopWindow.SetActive(true);
    }
}
