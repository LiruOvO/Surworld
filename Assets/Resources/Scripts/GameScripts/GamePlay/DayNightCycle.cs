using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal; 

//«м≥на осв≥тленн€ в залежност≥ в≥д часу дн€
public class DayNightCycle : MonoBehaviour
{
    [Header("Settings")]
    public Light2D globalLight; 
    public Gradient dayNightGradient;
    public TextMeshProUGUI timeText;
    public float dayDuration = 1440f; 

    public float currentTime = 0;
        public void UpdateVisuals()
    {
        if (globalLight != null)
            globalLight.color = dayNightGradient.Evaluate(currentTime);
    }

    void Update()
    {
        // «б≥льшуЇмо час
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