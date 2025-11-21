using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Скрипт для керування меню кастомізації гравця
public class MenuManager : MonoBehaviour
{
    public Texture2D cursorTexture;//Дефолтний курсор
    public GameObject settingsUI;
    public GameObject customizationUI;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); //Встановлення дефолтного курсору
    }
    private void Update()
    {
        //Відкриття закриття налаштувань
        if (settingsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) SettinsControl(false);
        else if (customizationUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) SettinsControl(true);
    }
    public void SettinsControl(bool state)
    {
        settingsUI.SetActive(state);
        customizationUI.SetActive(!state);
    }
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR//Для закриття гри в редакторі
        EditorApplication.ExitPlaymode();
#endif
    }


    //Збереження кастомізації гравця
    public PlayerCustomization[] parts;
    public TMP_InputField playerName;
    public GameObject attentionSign;

    public void SavePlayerCustomization()
    {
        foreach (var part in parts)
        {
            part.SaveSelectedOption();            
        }
        if (!string.IsNullOrWhiteSpace(playerName.text))
        {
            DataManager.Instance.playerName = playerName.text;
            SceneManager.LoadScene("GameWorld");            
        }
        else attentionSign.SetActive(true);
    }
    
}
