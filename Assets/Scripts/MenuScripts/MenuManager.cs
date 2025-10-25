using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Texture2D cursorTexture;//Дефолтний курсор
    
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto); //Встановлення дефолтного курсору
    }
}
