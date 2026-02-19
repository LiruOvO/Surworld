using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
//Скрипт для шкали здоров'я
public class HealthManager : MonoBehaviour
{
    public Slider healthSlider;
    public float currentHealth;


    private SpriteRenderer[] renderers;
    private Color[] originalColors;
    private void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();

        // запам'ятовуємо їхні кольори
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }

        healthSlider.value = currentHealth;
    }

    public void Heal(int health)
    {
        currentHealth += health;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
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


    IEnumerator DamageFlash()
    {
        // всі частини стають червоними
        foreach (var r in renderers)
            r.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        // повертаємо оригінальні кольори
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].color = originalColors[i];
    }
}
