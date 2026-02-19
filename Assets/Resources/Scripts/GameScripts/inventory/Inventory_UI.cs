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

            int slotIndex = -1;
            if (slotsMini.Contains(selectedSlot)) slotIndex = slotsMini.IndexOf(selectedSlot);

            //Якщо в слоту їжа то з'їсти її
            if (selectedSlot.slotCollectible != CollectableType.NONE)
            {
                Collectable itemData = ItemManager.Instance.GetItemByType(selectedSlot.slotCollectible);
                bool canBeEaten = itemData.isConsumable;
                if (canBeEaten)
                {
                    int nutrition = itemData.foodToRecover;
                    player.GetComponent<HungerManager>().Eat(nutrition);
                    player.inventory.Remove(slotIndex);
                    Refresh();
                    return;
                }
            }

            //Передача спрайтів колектібл для аніматора            
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

    public void SwapSlots(Slot_UI sourceUI, Slot_UI targetUI)
    {
        // Шукаємо індекс початкового слота в обох списках
        int sourceIndex = slots.IndexOf(sourceUI);
        if (sourceIndex == -1) sourceIndex = slotsMini.IndexOf(sourceUI);

        // Шукаємо індекс цільового слота в обох списках
        int targetIndex = slots.IndexOf(targetUI);
        if (targetIndex == -1) targetIndex = slotsMini.IndexOf(targetUI);

        // Якщо обидва індекси знайдені (не дорівнюють -1)
        if (sourceIndex != -1 && targetIndex != -1)
        {
            // Міняємо дані в логічному інвентарі гравця
            Inventory.Slot temp = player.inventory.slots[sourceIndex];
            player.inventory.slots[sourceIndex] = player.inventory.slots[targetIndex];
            player.inventory.slots[targetIndex] = temp;

            // Оновлюємо весь інтерфейс
            Refresh();

            // Оновлюємо спрайт у руках гравця, якщо поміняли активний слот
            if (sourceUI.itemIcon.color.a > 0.9f || targetUI.itemIcon.color.a > 0.9f)
            {
                // Якщо один зі слотів був виділений кольором (чорним), 
                // викликаємо виділення знову, щоб оновити предмет у руках
                if (sourceUI == selectedSlot) HandleSlotSelection(sourceUI);
                else if (targetUI == selectedSlot) HandleSlotSelection(targetUI);
            }
        }
    }


}
