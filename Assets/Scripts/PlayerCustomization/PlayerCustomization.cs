using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


//Скрипт для меню кастомізації гравця
public class PlayerCustomization : MonoBehaviour
{
    public SpriteRenderer bodyPart;
    public List<SpriteOption> options = new List<SpriteOption>();
    private int currentOption = 0;

    [System.Serializable] 
    public class SpriteOption
    {
        public Sprite[] sprites;
    }

    public void NextOption()
    {
        currentOption++;
        if (currentOption >= options.Count) 
        {
            currentOption = 0;
        }
        bodyPart.sprite = options[currentOption].sprites[12];
    }

    public void PrevOption()
    {
        currentOption--;
        if(currentOption < 0) 
        {
            currentOption = options.Count - 1;
        }
        bodyPart.sprite = options[currentOption].sprites[12];
    }


    
    public void SaveSelectedOption()
    {
        if (bodyPart.name == "Hair")
        {
            DataManager.Instance.selectedHair = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Eyes")
        {
            DataManager.Instance.selectedEyes = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Eyebrows")
        {
            DataManager.Instance.selectedEyebrows = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Head")
        {
            DataManager.Instance.selectedHead = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Hands")
        {
            DataManager.Instance.selectedHands = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Shirt")
        {
            DataManager.Instance.selectedShirt = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Trousers")
        {
            DataManager.Instance.selectedTrousers = options[currentOption].sprites;
        }
        else if (bodyPart.name == "Boots")
        {
            DataManager.Instance.selectedBoots = options[currentOption].sprites;
        }
    }
    }
