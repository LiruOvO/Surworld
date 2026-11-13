using UnityEngine;

//Взаємодія з НПС за допомогою клавіші "Е"

public class NPCInteractable : MonoBehaviour, IInteractable
{
    public GameObject interactPrompt;   // дочірній об'єкт "E - Talk"
    public float interactRadius = 1.5f;
    public NPCShops shop;
    public string npcName; // можна лишити порожнім

    private NPCDialogues dialogues;
    private GameObject player;

    private Transform pivot;
    
    void Start()
    {
        dialogues = FindFirstObjectByType<NPCDialogues>();
        player = GameObject.FindGameObjectWithTag("Player");

        // дочірній порожній об'єкт Pivot, якщо його немає, беремо позицію самого НПС
        pivot = transform.Find("Pivot");

        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    public Vector2 InteractPoint => pivot != null ? pivot.position : transform.position;

    void OnEnable() { InteractionManager.All.Add(this); }
    void OnDisable() { InteractionManager.All.Remove(this); SetPrompt(false); }

    public float InteractRadius => interactRadius;

    // поки відкритий діалог, підказки немає
    public bool CanInteract =>
    dialogues != null
    && !dialogues.dialogueArea.activeSelf
    && (shop == null || shop.shopWindow == null || !shop.shopWindow.activeSelf);


    public void SetPrompt(bool visible)
    {
        if (interactPrompt != null) interactPrompt.SetActive(visible);
    }

    public void Interact()
    {
        if (shop != null)
        {
            shop.OpenShop();
            return;
        }

        string nameToUse = npcName;
        if (string.IsNullOrEmpty(nameToUse))
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col == null) col = GetComponentInChildren<Collider2D>();
            nameToUse = col != null ? col.name : gameObject.name;
        }
        dialogues.TalkTo(nameToUse, player);
    }

}