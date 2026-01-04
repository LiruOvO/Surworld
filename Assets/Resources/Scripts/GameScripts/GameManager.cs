using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


//Менеджер курсору а також функцій кнопок
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
    public GameObject inventoryMiniUI;
    private void Update()
    {
        //Відкриття та закриття налаштувань гри
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingsUI.activeSelf) inventoryMiniUI.SetActive(false);
            else inventoryMiniUI.SetActive(true);
            inventoryUI.SetActive(false);            
            bool state = !settingsUI.activeSelf;
            ShowSettings(state);
        }

        //Відкриття інвентарю
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowInventory();
        }
        
    }
    
    //Відкриття та закриття інвентарю
    public void ShowInventory()
    {
        if (settingsUI.activeSelf) settingsUI.SetActive(false);
        if (!inventoryUI.activeSelf) inventoryMiniUI.SetActive(false);
        else inventoryMiniUI.SetActive(true);

        if (!inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(true);
            inventoryScript.Refresh();
        }
        else inventoryUI.SetActive(false);
    }


    //Функція для відкриття та закриття налаштувань гри
    public void ShowSettings(bool state)
    {
        settingsUI.SetActive(state);
        inventoryMiniUI.SetActive(!state);
    }



    //Функція вфдкриття та закритта кастомізації гравця
    public GameObject playerCustUi;
    public void ShowPlayerCustomization(bool state)
    {
        playerCustUi.SetActive(state);
        inventoryMiniUI.SetActive(!state);
    }
    //Збереження кастомізації гравця
    public PlayerCustomization[] parts;

    public void SavePlayerCustomization()
    {
        foreach (var part in parts)
        {
            part.SaveSelectedOption();
        }
        PlayerPartsAnimation saveParts = FindFirstObjectByType<PlayerPartsAnimation>();
        saveParts.SaveAll();
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
