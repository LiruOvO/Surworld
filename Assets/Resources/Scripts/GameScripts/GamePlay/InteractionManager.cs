using System.Collections.Generic;
using UnityEngine;

//Відповідає за взаємодію з об'єктами через клавішу "Е"

public interface IInteractable
{
    Vector2 InteractPoint { get; }
    float InteractRadius { get; }
    bool CanInteract { get; }
    void SetPrompt(bool visible);
    void Interact();
}

// Повісь на Player
public class InteractionManager : MonoBehaviour
{
    public static readonly List<IInteractable> All = new List<IInteractable>();
    public KeyCode interactKey = KeyCode.E;

    private Transform origin;
    private IInteractable current;

    void Start()
    {
        Transform pivot = transform.Find("Pivot");
        origin = pivot != null ? pivot : transform;
    }

    void Update()
    {
        if (current != null && !All.Contains(current)) current = null;

        IInteractable best = null;
        float bestDist = float.MaxValue;

        foreach (var item in All)
        {
            if (!item.CanInteract) continue;
            float d = Vector2.Distance(origin.position, item.InteractPoint);
            if (d <= item.InteractRadius && d < bestDist)
            {
                best = item;
                bestDist = d;
            }
        }

        if (best != current)
        {
            current?.SetPrompt(false);
            best?.SetPrompt(true);
            current = best;
        }

        if (current != null && Input.GetKeyDown(interactKey))
            current.Interact();
    }
}
