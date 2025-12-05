using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//Налаштування самого інветаря гравця, додавання предметів та їх видалення
[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public CollectableType type;
        public int count;

        public Sprite icon;
        public Sprite[] spritesForAniimation;
        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
        }

        public bool CanAddItem(CollectableType itemType, int itemMaxAllowed)
        {
            if (type == CollectableType.NONE) return true;
            if (type == itemType && count < itemMaxAllowed) return true;

            return false;
        }

        public void AddItem(Collectable item)
        {
            if (type == CollectableType.NONE)
            {
                this.spritesForAniimation = item.spritesForAnimation;
                this.type = item.type;
                this.icon = item.icon;
            }
            count++;
        }

        public void RemoveItem()
        {
            if(count > 0)
            {
                count--;
                if(count == 0)
                {
                    icon = null;
                    spritesForAniimation = null;
                    type = CollectableType.NONE;
                }
            }
        }
    }


    public List<Slot> slots = new List<Slot>();


    public Inventory(int numSlots) { 
        for(int i =0; i<numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }

    }

    //Повертає булеве значення чи може предмет бути доданим в інвентар
    public bool Add(Collectable item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.type == item.type && slot.CanAddItem(item.type, item.maxAllowed))
            {
                slot.AddItem(item);
                return true;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.type == CollectableType.NONE)
            {
                if (item.maxAllowed > 0)
                {
                    slot.AddItem(item);
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}
