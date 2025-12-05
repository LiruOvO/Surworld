using System.Collections.Generic;
using UnityEngine;

//Каталог усіх предметів,які можна зібрати. Прикріплений до гейм менеджера в грі.
//Корисна штука, замість того щоб префаб прикріпляти всюди, можна дістати префаб за допомогою колектібл типу
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public Collectable[] collectableItems;
    public Resource[] resourceItems;
    private Dictionary<CollectableType, Collectable> collectableItemDict = new Dictionary<CollectableType, Collectable>();
    private Dictionary<ResourceType, Resource> resourceItemDict = new Dictionary<ResourceType, Resource>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        foreach (Collectable item in collectableItems)
        {
            AddItem(item);
        }
        foreach (Resource item in resourceItems)
        {
            AddItem(item);
        }
    }

    private void AddItem(Collectable item)
    {
        if (!collectableItemDict.ContainsKey(item.type))
        {
            collectableItemDict.Add(item.type, item);
        }
    }
    private void AddItem(Resource item)
    {
        if (!resourceItemDict.ContainsKey(item.type))
        {
            resourceItemDict.Add(item.type, item);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        if (collectableItemDict.ContainsKey(type))
        {
            return collectableItemDict[type];
        }

        return null;
    }
    public Resource GetItemByType(ResourceType type)
    {
        if (resourceItemDict.ContainsKey(type))
            return resourceItemDict[type];

        return null;
    }
}
