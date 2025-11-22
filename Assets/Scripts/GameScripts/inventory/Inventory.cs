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
        public int maxAllowed;

        public Sprite icon;
        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            maxAllowed = 100;
        }

        public bool CanAddItem()
        {
            if (count < maxAllowed) return true;

            return false;
        }

        public void AddItem(Collectable item)
        {
            this.type = item.type;
            this.icon = item.icon;
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
            if (slot.type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return true;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.type == CollectableType.NONE)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}
