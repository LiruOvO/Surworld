using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonsManager : MonoBehaviour
{
    //Змінення розширення екрану
    public Toggle FullScreenToggle;
    public TMP_Dropdown ResolutionDropdown;
    Resolution[] allResolutions;
    int selectedResolution;
    List<Resolution> selRes = new List<Resolution>();
    bool isFullScreen = true;

    private void Start()
    {
        GetResolution();
    }

    public void GetResolution()
    {
        allResolutions = Screen.resolutions;

        List<string> resStrings = new List<string>();
        string newRes;
        Resolution currentRes = Screen.currentResolution;
        int currentResIndex = 0;
        foreach (Resolution res in allResolutions)
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if (!resStrings.Contains(newRes))
            {
                resStrings.Add(newRes);
                selRes.Add(res);

                if (res.width == currentRes.width && res.height == currentRes.height)
                {
                    currentResIndex = resStrings.Count - 1;
                }
            }
        }

        ResolutionDropdown.AddOptions(resStrings);
        ResolutionDropdown.value = currentResIndex;
        selectedResolution = ResolutionDropdown.value;
    }

    public void ChangeResolution()
    {
        selectedResolution = ResolutionDropdown.value;
        Screen.SetResolution(selRes[selectedResolution].width, selRes[selectedResolution].height, isFullScreen);
    }
    public void ChangeFullScreen()
    {
        isFullScreen = FullScreenToggle.isOn;
        Screen.SetResolution(selRes[selectedResolution].width, selRes[selectedResolution].height, isFullScreen);
    }


    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject mainUI;
    private void Update()
    {
        //Відкриття || закриття налаштувань
        if(settingsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) CloseSettings();
        else if(mainUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) OpenSettings();
    }




    public void PlayGame()
    {
        SceneManager.LoadScene("");
    }

    
    //Відкрити меню налаштувань
    public void OpenSettings()
    {
        settingsUI.SetActive(true);
        mainUI.SetActive(false);
    }

    //Вийти з меню налаштувань
    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        mainUI.SetActive(true);
    }

    //Кнопка виходу з гри
    public void ExitGame()
    {
        Application.Quit();

    #if UNITY_EDITOR//Для закриття гри в редакторі
            EditorApplication.ExitPlaymode();
    #endif
    }

    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }



    


}
