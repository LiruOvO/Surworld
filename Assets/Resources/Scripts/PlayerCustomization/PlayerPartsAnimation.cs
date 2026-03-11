using System.Collections;
using UnityEngine;


//Скрипт для витягування спрайтів кастомізації гравця з датаменеджеру та встаномлення анімації для них
public class PlayerPartsAnimation : MonoBehaviour
{
    public SpriteRenderer headRenderer;
    public SpriteRenderer handsRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer eyesRenderer;
    public SpriteRenderer eyebrowsRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer trousersRenderer;
    public SpriteRenderer bootsRenderer;

    public SpriteRenderer collectableRenderer;

    public Sprite[] head;
    public Sprite[] hands;
    public Sprite[] hair;
    public Sprite[] eyes;
    public Sprite[] eyebrows;
    public Sprite[] shirt;
    public Sprite[] trousers;
    public Sprite[] boots;

    public Sprite[] collectable;



    private void Start()
    {
        SaveAll();
    }

    //Присвоєння значень спрайтів з датаменеджеру
    public void SaveAll()
    {
        hair = DataManager.Instance.selectedHair;
        eyes = DataManager.Instance.selectedEyes;
        eyebrows = DataManager.Instance.selectedEyebrows;
        head = DataManager.Instance.selectedHead;
        hands = DataManager.Instance.selectedHands;
        shirt = DataManager.Instance.selectedShirt;
        trousers = DataManager.Instance.selectedTrousers;
        boots = DataManager.Instance.selectedBoots;
    }

    //Івент для анімації всіх спрайтів в аніматорі
    public void SetFrame(int frameIndex)
    {
        hairRenderer.sprite = hair[frameIndex];
        headRenderer.sprite = head[frameIndex];
        handsRenderer.sprite = hands[frameIndex];
        eyesRenderer.sprite = eyes[frameIndex];
        eyebrowsRenderer.sprite = eyebrows[frameIndex];
        shirtRenderer.sprite = shirt[frameIndex];
        trousersRenderer.sprite = trousers[frameIndex];
        bootsRenderer.sprite = boots[frameIndex];
    }

    //Анімація для колектіблс
    public void SetCollectibleSprites(Sprite[] newCollectableSprites)
    {
        // Встановлюємо новий масив спрайтів
        collectable = newCollectableSprites;
    }
    public void SetColFrame(int frameIndex)
    {
        if (collectable != null && collectable.Length > 0)
        {
            if (frameIndex >= 0 && frameIndex < collectable.Length)
            {
                collectableRenderer.sprite = collectable[frameIndex];
            }
        }
        else
        {
            collectableRenderer.sprite = null;
        }
    }


}
