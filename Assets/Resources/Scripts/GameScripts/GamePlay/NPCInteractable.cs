using UnityEngine;

//Взаємодія з НПС за допомогою клавіші "Е"

public class NPCInteractable : MonoBehaviour, IInteractable
{
    public GameObject interactPrompt;   // дочірній об'єкт "E - Talk"
    public float interactRadius = 1.5f;

    private NPCDialogues dialogues;
    private GameObject player;

    void Start()
    {
        dialogues = FindFirstObjectByType<NPCDialogues>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    void OnEnable() { InteractionManager.All.Add(this); }
    void OnDisable() { InteractionManager.All.Remove(this); SetPrompt(false); }

    public Vector2 InteractPoint => transform.position;
    public float InteractRadius => interactRadius;

    // поки відкритий діалог, підказки немає
    public bool CanInteract => dialogues != null && !dialogues.dialogueArea.activeSelf;

    public void SetPrompt(bool visible)
    {
        if (interactPrompt != null) interactPrompt.SetActive(visible);
    }

    public void Interact()
    {
        dialogues.TalkTo(gameObject.name, player);
    }
}