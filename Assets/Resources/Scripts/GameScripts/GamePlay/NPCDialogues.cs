using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPCDialogues : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLines;
    byte dialogue = 0;

    public GameObject dialogueArea;
    public TMP_Text playerName;
    public TMP_Text npcName;
    public TMP_Text conversationArea;
    public Button mainActionButton;

    Player playerScript;
    Inventory_UI inventory_UI;

    private void Awake()
    {
        LoadDialogue();
    }

    public void TalkTo(string NPCName, GameObject player)
    {
        mainActionButton.onClick.RemoveAllListeners();
        playerScript = player.GetComponent<Player>();
        inventory_UI = FindFirstObjectByType<Inventory_UI>();

        if (NPCName == "NPC_Jolie")
        {
            switch(dialogue)
            {
                case 0:
                    mainActionButton.onClick.AddListener(CloseDialogue);
                    dialogueArea.SetActive(true);
                    playerName.text = DataManager.Instance.playerName;
                    npcName.text = NPCName;
                    conversationArea.text = dialogueLines[0];
                    GiveItem(CollectableType.WOODEN_PICKAXE);
                    dialogue++;
                    SaveDialogue();
                    break;
                case 1:
                    mainActionButton.onClick.AddListener(GoToTheSaloon);
                    dialogueArea.SetActive(true);
                    playerName.text = DataManager.Instance.playerName;
                    npcName.text = NPCName;
                    conversationArea.text = dialogueLines[1];                    
                    break;
            }
        }
    }

    public void CloseDialogue()
    {
       dialogueArea.SetActive(false);
    }
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
        int savedState = PlayerPrefs.GetInt(saveKey, 0);
        dialogue = (byte)savedState;
    }

    //Äëÿ Äæîë³
    public void GoToTheSaloon()
    {
        dialogueArea.SetActive(false);
        SaveController saveController = Object.FindFirstObjectByType<SaveController>();
        saveController.SaveGame();
        SceneManager.LoadScene(1);        
    }
}
