using UnityEngine;

//Ресурс який можна добувати, з нього випадають collectible
public class Resource : MonoBehaviour
{
    public enum ResourceType { Stone, Wood }
    public ResourceType type;

}
