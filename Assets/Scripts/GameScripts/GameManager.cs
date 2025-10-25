using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D cursorTexture;//Дефолтний курсор
    [SerializeField] private GameObject settingsUI;
    
    void Start()
    {
        //Курсор прихований та зафіксований по центру екрану
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); //Встановлення дефолтного курсору
    }

    
    private void Update()
    {
        //Відкриття та закриття налаштувань гри
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool state = !settingsUI.activeSelf;
            ShowSettings(state);
        }
        if(!settingsUI.activeSelf) Cursor.visible = false;
    }


    //Функція для відкриття та закриття налаштувань гри
    public void ShowSettings(bool state)
    {
        settingsUI.SetActive(state);
        Cursor.visible = state;
    }

    //Кнопка виходу з гри
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR//Для закриття гри в редакторі
        EditorApplication.ExitPlaymode();
#endif
    }

}
