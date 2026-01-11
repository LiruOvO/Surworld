using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal; 

//Зміна освітлення в залежності від часу дня
public class DayNightCycle : MonoBehaviour
{
    [Header("Settings")]
    public Light2D globalLight; 
    public Gradient dayNightGradient;
    public TextMeshProUGUI timeText;
    public float dayDuration = 1440f;


    public Light2D[] streetLights;
    public float turnOnTime = 0.75f;//18:00
    public float turnOffTime = 0.167f; //4:00
    public float maxIntensity = 4.0f; 
    public float fadeSpeed = 0.5f;

    public float currentTime = 0;
    public void UpdateVisuals()
    {
        if (globalLight != null)
            globalLight.color = dayNightGradient.Evaluate(currentTime);

        ToggleStreetLights();
    }

    //Увімкнення ліхтарів
    void ToggleStreetLights()
    {
        //перевіряємо, чи зараз ніч 
        bool isNight = currentTime >= turnOnTime || currentTime <= turnOffTime;
        float targetIntensity = isNight ? maxIntensity : 0f;

        foreach (Light2D light in streetLights)
        {
            if (light != null)
            {
                // Переконайтеся, що об'єкт увімкнений, щоб ми бачили зміну інтенсивності
                light.enabled = true;

                // Плавна зміна поточної інтенсивності до цільової
                light.intensity = Mathf.MoveTowards(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);

                // Опціонально: вимикаємо компонент повністю, якщо інтенсивність стала 0, для економії ресурсів
                if (light.intensity <= 0 && !isNight) light.enabled = false;
            }
        }
    }


    void Update()
    {
        // Збільшуємо час
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime >= 1f) currentTime = 0f;
                
        UpdateVisuals();
        DisplayTime();
    }
    
    void DisplayTime()
    {
        float totalHours = currentTime * 24f;

        int hours = Mathf.FloorToInt(totalHours);
        int minutes = Mathf.FloorToInt((totalHours - hours) * 60f);

        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }
}