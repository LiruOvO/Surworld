using UnityEngine;

//Для збереження предметів в чанках
public class ChunkableEntity : MonoBehaviour
{
    [SerializeField] private int uniqueID = 0;
    [SerializeField] private string parentName = "";
    public int UniqueID
    {
        get => uniqueID;
        set => uniqueID = value;
    }
    public string ParentName
    {
        get => parentName;
        set => parentName = value;
    }

    public bool IsDestroyed { get; private set; } = false;
    public static int nextID = 1;

    private void Awake()
    {
        if (uniqueID == 0) UniqueID = nextID++;
        if (transform.parent != null)
            parentName = transform.parent.name;
        ChunkHandler.Instance?.RegisterEntity(gameObject);
    }

    public void DestroyAndSave()
    {
        IsDestroyed = true;
        ChunkHandler.Instance?.UnregisterEntity(gameObject);
        Destroy(gameObject);
        WorldStorage.Instance?.SaveWorld();
    }
}
