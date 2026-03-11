using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject soundsOn;
    [SerializeField] private GameObject soundsOff;

    private void Update()
    {
        //Відкриття || закриття налаштувань
        if (settingsUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) SettinsControl(false);
        else if (mainUI.activeSelf && Input.GetKeyDown(KeyCode.Escape)) SettinsControl(true);
    }

    //Відкрити/закрити меню налаштувань
    public void SettinsControl(bool state)
    {
        settingsUI.SetActive(state);
        mainUI.SetActive(!state);
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

    //Кнопка запуску гри або меню кастомізації
    public void Play(string sceneName)
    {
        if(string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(PlayAndLoad("PlayerCustomization"));
        }else StartCoroutine(PlayAndLoad(sceneName));
    }
    IEnumerator PlayAndLoad(string sceneName) //затримка щоб звук встигнув прозвучати
    {
        yield return new WaitForSeconds(1.2f);
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

    /*public void SoundsOff()
    {
        AudioListener.pause = true;
        soundsOff.SetActive(true);
        soundsOn.SetActive(false);
    }

    public void SoundsOn()
    {
        AudioListener.pause = false;
        soundsOff.SetActive(false);
        soundsOn.SetActive(true);
    }*/

}
