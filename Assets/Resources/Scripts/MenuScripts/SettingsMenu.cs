using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

//Функціональність усіх кнопок в меню налаштувань

public class SettingsMenu : MonoBehaviour
{
    //Змінення розширення екрану
    public Toggle FullScreenToggle;
    public TMP_Dropdown ResolutionDropdown;
    Resolution[] allResolutions;    
    
    int selectedResolution;
    List<Resolution> selRes = new List<Resolution>();
    bool isFullScreen = true;


    [Header("Volume Settings")]
    public Scrollbar volumeScrollbar;

    private void Start()
    {
        GetResolution();

        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volumeScrollbar.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeScrollbar.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
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

    //Видалення збереження
    public void ResetSavings()
    {
        string saveWorldLocation = Path.Combine(Application.persistentDataPath, "world.json");
        if (File.Exists(saveWorldLocation))
        {
            File.Delete(saveWorldLocation);
        }
        string saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
        }
        SceneManager.LoadScene(0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
  

}
