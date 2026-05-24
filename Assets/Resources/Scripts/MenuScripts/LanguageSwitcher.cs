using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    // Зберігаємо вибрану мову між сесіями
    private const string LANG_KEY = "SelectedLanguage";

    private void Start()
    {
        // Завантажити збережену мову при старті
        string saved = PlayerPrefs.GetString(LANG_KEY, "en");
        StartCoroutine(SetLocale(saved));
    }

    // Викликається при натисканні кнопки Language
    public void ToggleLanguage()
    {
        string current = LocalizationSettings.SelectedLocale.Identifier.Code;
        string next = (current == "uk-UA") ? "en" : "uk-UA";
        StartCoroutine(SetLocale(next));
    }

    private IEnumerator SetLocale(string code)
    {
        yield return LocalizationSettings.InitializationOperation;

        var locale = LocalizationSettings.AvailableLocales
            .Locales.Find(l => l.Identifier.Code == code);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString(LANG_KEY, code);
            PlayerPrefs.Save();
        }
    }
}