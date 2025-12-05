using UnityEngine;


//Тут описується які дані потрібно зберігати при виході з гри
[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string scene;

    public string playerName;
    public string[] hairSprites;
    public string[] eyesSprites;
    public string[] eyebrowsSprites;
    public string[] headSprites;
    public string[] handsSprites;
    public string[] shirtSprites;
    public string[] trousersSprites;
    public string[] bootsSprites;

    public Inventory playerInventory;
}
