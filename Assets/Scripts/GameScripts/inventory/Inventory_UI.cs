using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//Оновлення інвентарю гравця
public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public List<Slot_UI> slots = new List<Slot_UI>();
    public List<Slot_UI> slotsMini = new List<Slot_UI>();    


    public void Refresh()
    {        
        if (slots.Count == player.inventory.slots.Count)
        {
           for (int i = 0; i < slots.Count; i++)
           {
                if (player.inventory.slots[i].type != CollectableType.NONE)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                    if(i<7) slotsMini[i].SetItem(player.inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                    if (i < 7) slotsMini[i].SetEmpty();
                }                
           }
        }
    }

    public void Remove(int slotID)
    {
        Collectable itemToDrop = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[slotID].type);

        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    }
}
