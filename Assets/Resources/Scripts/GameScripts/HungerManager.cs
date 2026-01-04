using UnityEngine;
using UnityEngine.UI;

//Скрипт для шкали голоду
public class HungerManager : MonoBehaviour
{
    public Slider hungerSlider;
    public Slider healthManager;
    public float currentHunger;
    public float looseSpeed = 5f;

    private void Start()
    {
        hungerSlider.value = currentHunger;
        InvokeRepeating("IncreaseHunder", looseSpeed, looseSpeed); //коли починається виклик після старту, через скільки часу повторюється виклик
    }

    //Зменшення шкали їжі на 1 кожні 5 секунд 
    public void IncreaseHunder()
    {
        if(currentHunger > 0)
        {
            currentHunger -= 1;
            currentHunger = Mathf.Clamp(currentHunger, 0, hungerSlider.maxValue);
            hungerSlider.value = currentHunger;    
        }
        if (currentHunger <= 0)
        {
            Starving();
        }
    }

    //Зменшення хп коли високий голод
    public void Starving()
    {
        if(currentHunger == 0)
        {
            GetComponent<HealthManager>().currentHealth -= 1f;
            healthManager.value = GetComponent<HealthManager>().currentHealth;            
        }        
    }

    //Їсти
    public void Eat(int amount)
    {
        if(currentHunger < hungerSlider.maxValue)
        {
            currentHunger += amount;
            currentHunger = Mathf.Clamp(currentHunger, 0, hungerSlider.maxValue);
            hungerSlider.value = currentHunger;
        }        
    }
}
