using TMPro;
using UnityEngine;

public class NPCDialogues : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] dialogueLines;
    byte dialogue = 0;

    public GameObject dialogueArea;
    public TMP_Text playerName;
    public TMP_Text npcName;
    public TMP_Text conversationArea;

    private void Awake()
    {
        LoadDialogue();
    }

    public void TalkTo(string NPCName, GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();
        Inventory_UI inventory_UI = FindFirstObjectByType<Inventory_UI>();
        if (NPCName == "NPC_Jolie")
        {
            switch(dialogue)
            {
                case 0: 
                    dialogueArea.SetActive(true);
                    playerName.text = DataManager.Instance.playerName;
                    npcName.text = NPCName;
                    conversationArea.text = dialogueLines[0];

                    Collectable[] tools = new Collectable[2]; 
                    tools[0]= ItemManager.Instance.GetItemByType(CollectableType.WOODEN_PICKAXE);
                    tools[1] = ItemManager.Instance.GetItemByType(CollectableType.STONE_PICKAXE);
                    playerScript.inventory.Add(tools[0]);
                    playerScript.inventory.Add(tools[1]);
                    inventory_UI.Refresh();
                    dialogue++;
                    SaveDialogue();
                    break;
                case 1:

                    break;
            }
        }
    }

    public void CloseDialogue()
    {
        dialogueArea.SetActive(false);
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
}
