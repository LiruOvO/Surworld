using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;  
using System.Collections;                

public class NPCDialogues : MonoBehaviour
{
    byte dialogue = 0;

    public GameObject dialogueArea;
    public TMP_Text playerName;
    public TMP_Text npcName;
    public TMP_Text conversationArea;
    public Button mainActionButton;

    Player playerScript;
    Inventory_UI inventory_UI;
    public GameManager gameManager;

    private void Awake()
    {
        LoadDialogue();
    }

    // Новий метод — отримати перекладений рядок
    private IEnumerator ShowLocalizedDialogue(string key)
    {
        var op = LocalizationSettings.StringDatabase
            .GetLocalizedStringAsync("Dialogues", key);
        yield return op;
        conversationArea.text = op.Result;
    }

    public void TalkTo(string NPCName, GameObject player)
    {
        mainActionButton.onClick.RemoveAllListeners();
        playerScript = player.GetComponent<Player>();
        inventory_UI = FindFirstObjectByType<Inventory_UI>();

        if (NPCName == "NPC_Jolie")
        {
            switch (dialogue)
            {
                case 0:
                    mainActionButton.onClick.AddListener(CloseDialogue);
                    dialogueArea.SetActive(true);
                    playerName.text = DataManager.Instance.playerName;
                    npcName.text = NPCName;
                    StartCoroutine(ShowLocalizedDialogue("NPC_jolie_01")); 
                    GiveItem(CollectableType.WOODEN_PICKAXE);
                    GiveItem(CollectableType.WOODEN_AXE);
                    dialogue++;
                    SaveDialogue();
                    break;
                case 1:
                    mainActionButton.onClick.AddListener(GoToTheSaloon);
                    dialogueArea.SetActive(true);
                    playerName.text = DataManager.Instance.playerName;
                    npcName.text = NPCName;
                    StartCoroutine(ShowLocalizedDialogue("NPC_jolie_02")); 
                    break;
            }
        }
    }


    public void CloseDialogue() => dialogueArea.SetActive(false);

    public void GiveItem(CollectableType item)
    {
        Collectable tools = ItemManager.Instance.GetItemByType(item);
        playerScript.inventory.Add(tools);
        inventory_UI.Refresh();
    }

    public void SaveDialogue()
    {
        string saveKey = "DialogueState_" + gameObject.name;
        PlayerPrefs.SetInt(saveKey, dialogue);
        PlayerPrefs.Save();
    }

    public void LoadDialogue()
    {
        string saveKey = "DialogueState_" + gameObject.name;
        dialogue = (byte)PlayerPrefs.GetInt(saveKey, 0);
    }

    public void GoToTheSaloon()
    {
        dialogueArea.SetActive(false);
        gameManager.ShowPlayerCustomization(true);
    }

    public void SaveChanges()
    {
        SaveController saveController = Object.FindFirstObjectByType<SaveController>();
        saveController.SaveGame();
        gameManager.ShowPlayerCustomization(false);
        gameManager.SavePlayerCustomization();
    }
}