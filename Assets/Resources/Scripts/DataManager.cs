using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    //Кастомізація гравця
    public Sprite[] selectedHair;
    public Sprite[] selectedEyes;
    public Sprite[] selectedEyebrows;
    public Sprite[] selectedHead;
    public Sprite[] selectedHands;
    public Sprite[] selectedShirt;
    public Sprite[] selectedTrousers;
    public Sprite[] selectedBoots;


    //Ім'я гравця
    public string playerName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    /*[System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void SaveInfo()
    {
        SaveData saveData = new SaveData();
        //saveData.name = bestPlayerName;
        //saveData.score = score;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(UnityEngine.Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInfo()
    {
        string path = UnityEngine.Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            //bestPlayerName = saveData.name;
            //score = saveData.score;
        }
    }*/
}
