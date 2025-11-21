using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
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
}
