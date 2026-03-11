using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Sleep : MonoBehaviour
{
    public GameObject optionsMenu;
    public Image fadeImage;

    private void OnMouseDown()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 2f) optionsMenu.SetActive(true);
    }

    // Цей метод признач кнопці "ТАК" у меню
    public void StartSleeping()
    {
        optionsMenu.SetActive(false); // Закриваємо меню відразу
        StartCoroutine(SleepWithFade());
    }

    private IEnumerator SleepWithFade()
    {
        fadeImage.GameObject().SetActive(true);
        float duration = 1.5f;
        float elapsed = 0f;

        // 1. Плавне затемнення
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(0, 1, elapsed / duration);
            fadeImage.color = c;
            yield return null;
        }

        // 2. Логіка сну (виконується, поки екран чорний)
        ApplySleepEffects();

        yield return new WaitForSeconds(1f); // Пауза в темряві

        // 3. Плавне повернення
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(1, 0, elapsed / duration);
            fadeImage.color = c;
            yield return null;
        }
        fadeImage.GameObject().SetActive(false);
    }

    private void ApplySleepEffects()
    {
        DayNightCycle timeSystem = FindFirstObjectByType<DayNightCycle>();
        if (timeSystem != null)
        {
            // Додаємо 8 годин (8/24 = 0.333 від циклу)
            timeSystem.currentTime += 0.333f;
            if (timeSystem.currentTime >= 1f) timeSystem.currentTime -= 1f;

            timeSystem.UpdateVisuals();
            timeSystem.DisplayTime();
        }
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
}