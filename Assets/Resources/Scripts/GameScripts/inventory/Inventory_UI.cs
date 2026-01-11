using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//Оновлення інвентарю гравця
public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;
    public PlayerPartsAnimation playerPartsAnimation;
    public PlayerInteractions interactions;
    public List<Slot_UI> slots = new List<Slot_UI>();
    public List<Slot_UI> slotsMini = new List<Slot_UI>();


    private Slot_UI selectedSlot = null;
    //Виділення натиснутого слота
    public void HandleSlotSelection(Slot_UI clickedSlot)
    {
        //Скидання кольору для всіх слотів
        foreach (Slot_UI slot in slots)
        {
            slot.ResetColor();
        }
        foreach (Slot_UI slot in slotsMini)
        {
            slot.ResetColor();
        }

        if (clickedSlot == selectedSlot) {
            selectedSlot = null;//прибирання кольору зі слота
            interactions.selectedItem = CollectableType.NONE;
            playerPartsAnimation.SetCollectibleSprites(null);
        }
        else//виділення слота
        {
            clickedSlot.SelectColor();
            selectedSlot = clickedSlot;
            interactions.selectedItem = selectedSlot.slotCollectible; //передає який предмет в руці

            //Передача спрайтів колектібл для аніматора
            int slotIndex = -1;
            if (slotsMini.Contains(selectedSlot)) slotIndex = slotsMini.IndexOf(selectedSlot);
            if (slotIndex != -1 && slotIndex < player.inventory.slots.Count)
            {
                Inventory.Slot inventorySlot = player.inventory.slots[slotIndex];
                if (inventorySlot.spritesForAniimation != null)
                {
                    playerPartsAnimation.SetCollectibleSprites(inventorySlot.spritesForAniimation);
                }
                else
                {
                    playerPartsAnimation.SetCollectibleSprites(null);
                }
            }
        }
    }


    //Оновлення ЮІ інвентарю
    public void Refresh()
    {        
        if (slots.Count == player.inventory.slots.Count)
        {
           for (int i = 0; i < slots.Count; i++)
           {
                if (player.inventory.slots[i].type != CollectableType.NONE)
                {
                    slots[i].SetItem(player.inventory.slots[i]);
                    if (i < 7) 
                    {
                        slotsMini[i].SetItem(player.inventory.slots[i]);
                    }
                }
                else
                {
                    slots[i].SetEmpty();
                    if (i < 7) slotsMini[i].SetEmpty();
                }                
           }
        }
    }

    //Викидання предмету з інвентаря
    public void Remove(int slotID)
    {
        Collectable itemToDrop = ItemManager.Instance.GetItemByType(player.inventory.slots[slotID].type);

        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
    }

}
