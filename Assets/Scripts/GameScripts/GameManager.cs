using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D cursorTexture;//Дефолтний курсор
    [SerializeField] private GameObject settingsUI;
    public Inventory_UI inventoryScript;

    void Start()
    {
        //Курсор прихований та зафіксований по центру екрану
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); //Встановлення дефолтного курсору
    }


    public GameObject inventoryUI;
    private void Update()
    {
        //Відкриття та закриття налаштувань гри
        if (Input.GetKeyDown(KeyCode.Escape))
        {      
            inventoryUI.SetActive(false);
            bool state = !settingsUI.activeSelf;
            ShowSettings(state);
        }
        if(settingsUI.activeSelf || inventoryUI.activeSelf) Cursor.visible = true; else Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.Tab))//Відкриття інвентарю
        {
            if(settingsUI.activeSelf) settingsUI.SetActive(false);
            inventoryScript.ToggleInventory();
        }
    }


    //Функція для відкриття та закриття налаштувань гри
    public void ShowSettings(bool state)
    {
        settingsUI.SetActive(state);
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
