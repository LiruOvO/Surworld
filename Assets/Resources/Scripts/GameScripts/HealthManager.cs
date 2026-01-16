using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

//Скрипт для шкали здоров'я
public class HealthManager : MonoBehaviour
{
    public Slider healthSlider;
    public float currentHealth;

    private void Start()
    {
        healthSlider.value = currentHealth;
    }
    private void Update()
    {
        Die();
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            if (this.CompareTag("Player"))
            {
                currentHealth = 100f; 
                healthSlider.value = 100f;

                GetComponent<HungerManager>().currentHunger = 100f;
                GetComponent<HungerManager>().hungerSlider.value = 100f;

                

                transform.position = new Vector2(-19.5f, 46f);
                GetComponent<Player>().inventory.ClearInventory();

                Collectable starterPickaxe = ItemManager.Instance.GetItemByType(CollectableType.WOODEN_PICKAXE);
                GetComponent<Player>().inventory.Add(starterPickaxe);

                FindFirstObjectByType<Inventory_UI>().Refresh();

            }
            else
            {

                Destroy(this, 2);
            }
        }
    }
}
