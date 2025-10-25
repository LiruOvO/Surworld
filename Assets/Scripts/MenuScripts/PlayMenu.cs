using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject soundsOn;
    [SerializeField] private GameObject soundsOff;

    private void Update()
    {
        //Відкриття || закриття налаштувань
        if (settingsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) CloseSettings();
        else if (mainUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) OpenSettings();        
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

    public void SoundsChanger()
    {
        //Відкриття || закриття налаштувань
        if (soundsOn.activeSelf)
        {
            soundsOn.SetActive(false);
            soundsOff.SetActive(true);
        }else
        {
            soundsOn.SetActive(true);
            soundsOff.SetActive(false);
        }
    }

    //Кнопка запуску гри
    public void Play(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
