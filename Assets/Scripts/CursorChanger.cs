using UnityEngine;
using UnityEngine.EventSystems;

//«м≥нюЇ курсор при наведенн≥ на об'Їкт
public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorPointed;
    public Texture2D cursorIdle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorPointed, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorIdle, Vector2.zero, CursorMode.Auto);
    }
}
