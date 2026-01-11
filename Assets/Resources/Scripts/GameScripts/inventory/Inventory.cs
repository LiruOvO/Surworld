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
        public Collectable itemData;
        public int count;

        public Sprite icon;
        public Sprite[] spritesForAniimation;
        public Slot()
        {
            itemData = null;    
            count = 0;
        }

        public bool CanAddItem(CollectableType itemType, int itemMaxAllowed)
        {
            if (itemData == null) return true;
            if (itemData.type == itemType && count < itemMaxAllowed) return true;

            return false;
        }

        public void AddItem(Collectable item)
        {
            if (itemData == null)
            {
                this.itemData = item;
            }
            count++;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    itemData = null;
                }
            }
        }
    }


    public List<Slot> slots = new List<Slot>();


    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
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
            if (slot.itemData != null && slot.itemData.type == item.type && slot.CanAddItem(item.type, item.maxAllowed))
            {
                slot.AddItem(item);
                return true;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemData == null)
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

    //Повністю очищає інвентар
    public void ClearInventory()
    {
        foreach (Slot slot in slots)
        {
            slot.count = 0;
            slot.itemData = null;
        }
    }
}
