using UnityEngine;


//«м≥нюЇ пор€док в шарах дл€ об'Їкту, коли заходить гравець
public class SortingSwitch : MonoBehaviour
{
    private const int PLAYER_IN_FRONT_ORDER = 3;
    private const int PLAYER_BEHIND_ORDER = 6;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer objectRenderer = GetComponent<SpriteRenderer>();

            if (objectRenderer != null)
            {
                objectRenderer.sortingOrder = PLAYER_BEHIND_ORDER;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();

            if (playerRenderer != null)
            {
                playerRenderer.sortingOrder = PLAYER_IN_FRONT_ORDER;
            }
        }
    }
}
