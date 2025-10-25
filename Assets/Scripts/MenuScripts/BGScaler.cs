using UnityEngine;
using UnityEngine.UI;


// Якийсь код від гпт для змінення масштабу фону під кожен розмір екрану
[RequireComponent(typeof(Image))]
public class UIScalerBackground : MonoBehaviour
{
    private Image bgImage;
    private RectTransform rt;

    void Awake()
    {
        bgImage = GetComponent<Image>();
        rt = GetComponent<RectTransform>();

        if (bgImage != null)
        {
            bgImage.preserveAspect = true; // зберігаємо пропорції
        }
    }

    void Update()
    {
        FitToScreen();
    }

    void FitToScreen()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float imageRatio = (float)bgImage.sprite.bounds.size.x / bgImage.sprite.bounds.size.y;

        if (screenRatio >= imageRatio)
        {
            // Екран ширший, ніж картинка → розтягуємо по ширині
            rt.sizeDelta = new Vector2(Screen.width, Screen.width / imageRatio);
        }
        else
        {
            // Екран вищий → розтягуємо по висоті
            rt.sizeDelta = new Vector2(Screen.height * imageRatio, Screen.height);
        }
    }
}