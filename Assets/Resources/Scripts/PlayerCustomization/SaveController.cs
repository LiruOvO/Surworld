using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

//Зберігає дані в джсон при виході з гри
public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void LoadDataOnClick()
    {
        StartCoroutine(LoadGame());
    }

    //Збереження даних
    public void SaveGame()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Player player = playerObject != null ? playerObject.GetComponent<Player>() : null;

        SaveData saveData = new SaveData
        {
            scene = SceneManager.GetActiveScene().name
        };

        //збереження позиції та інвентаря
        saveData.playerName = DataManager.Instance.playerName;
        if (player != null)
        {
            saveData.playerPosition = player.transform.position;
            saveData.playerInventory = player.inventory;
        }

        //ім'я
        saveData.playerName = DataManager.Instance.playerName;

        //здоров'я та їжа
        saveData.health = playerObject.GetComponent<HealthManager>().currentHealth;
        saveData.hunger = playerObject.GetComponent<HungerManager>().currentHunger;
        saveData.coins = playerObject.GetComponent<Player>().GetCoins();

        //зовнішність
        saveData.hairSprites = SpritesToNames(DataManager.Instance.selectedHair, "Hair");
        saveData.eyesSprites = SpritesToNames(DataManager.Instance.selectedEyes, "Eyes");
        saveData.eyebrowsSprites = SpritesToNames(DataManager.Instance.selectedEyebrows, "Eyebrows");
        saveData.headSprites = SpritesToNames(DataManager.Instance.selectedHead, "Head");
        saveData.handsSprites = SpritesToNames(DataManager.Instance.selectedHands, "Hands");
        saveData.shirtSprites = SpritesToNames(DataManager.Instance.selectedShirt, "Shirt");
        saveData.trousersSprites = SpritesToNames(DataManager.Instance.selectedTrousers, "Trousers");
        saveData.bootsSprites = SpritesToNames(DataManager.Instance.selectedBoots, "Boots");
        
        if (playerObject != null) saveData.playerPosition = playerObject.transform.position;
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    //Завантаження даних
    private IEnumerator LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(saveData.scene);//сцена          
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); //координати гравця
            Player player = playerObject != null ? playerObject.GetComponent<Player>() : null;
            if (playerObject != null) playerObject.transform.position = saveData.playerPosition;
            var dm = DataManager.Instance;

            //здоров'я та їжа
            HealthManager hm = playerObject.GetComponent<HealthManager>();
            HungerManager hnm = playerObject.GetComponent<HungerManager>();
            hm.currentHealth = saveData.health;
            hnm.currentHunger = saveData.hunger;
            hm.healthSlider.value = saveData.health;
            hnm.hungerSlider.value = saveData.hunger;
            //монети
            Player pl = playerObject.GetComponent<Player>();
            pl.AddMoney(saveData.coins);
            pl.coinsAmmount.text = saveData.coins.ToString();


            //зовнішність
            dm.playerName = saveData.playerName;
            dm.selectedHair = NamesToSprites(saveData.hairSprites);
            dm.selectedEyes = NamesToSprites(saveData.eyesSprites);
            dm.selectedEyebrows = NamesToSprites(saveData.eyebrowsSprites);
            dm.selectedHead = NamesToSprites(saveData.headSprites);
            dm.selectedHands = NamesToSprites(saveData.handsSprites);
            dm.selectedShirt = NamesToSprites(saveData.shirtSprites);
            dm.selectedTrousers = NamesToSprites(saveData.trousersSprites);
            dm.selectedBoots = NamesToSprites(saveData.bootsSprites);

            //завантаження інвентарю
            if (player != null && saveData.playerInventory != null)
            {
                player.inventory = saveData.playerInventory;
                //оновлення UI після завантаження інвентарю
                Inventory_UI inventoryUI = FindFirstObjectByType<Inventory_UI>();
                if (inventoryUI != null)
                {
                    inventoryUI.player = player; 
                    inventoryUI.Refresh();
                }
            }
            //оновлення анімацій
            if (playerObject != null)
            {
                PlayerPartsAnimation animController = playerObject.GetComponent<PlayerPartsAnimation>();
                if (animController != null)
                {
                    animController.SaveAll();
                }
            }
        }
        else SaveGame();       
    }



    //Збереження коли гравець виходить з гри
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    //Зберігає назви спрайтів гравця
    public static string[] SpritesToNames(Sprite[] sprites, string folder)
    {
        string[] names = new string[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
            names[i] = "Sprites/Player/" + folder + "/" + sprites[i].name;
        return names;
    }

    //Завантажує спрайти гравця
    private Sprite[] NamesToSprites(string[] names)
    {
        Sprite[] sprites = new Sprite[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            string fullSpriteName = names[i];

            int lastSlashIndex = fullSpriteName.LastIndexOf('/');
            int lastUnderscoreIndex = fullSpriteName.LastIndexOf('_');
            string resourcePath = fullSpriteName;

            if (lastUnderscoreIndex > lastSlashIndex)
            {
                resourcePath = fullSpriteName.Substring(0, lastUnderscoreIndex);
            }
            Sprite[] allSprites = Resources.LoadAll<Sprite>(resourcePath);
            string targetSpriteName = fullSpriteName.Substring(lastSlashIndex + 1); 

            Sprite foundSprite = null;
            foreach (Sprite s in allSprites)
            {
                if (s.name == targetSpriteName)
                {
                    foundSprite = s;
                    break;
                }
            }

            sprites[i] = foundSprite;

            if (sprites[i] == null)
            {
                Debug.LogWarning("Sprite not found: " + fullSpriteName + " (Tried to load from: " + resourcePath + ")");
            }
        }
        return sprites;
    }

}
