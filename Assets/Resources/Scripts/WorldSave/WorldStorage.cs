using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//Збереження світу
public class WorldStorage : MonoBehaviour
{
    public static WorldStorage Instance;
    [SerializeField] private string saveFileName = "world.json";
    public List<GameObject> prefabList = new();

    private Dictionary<string, GameObject> prefabLibrary;
    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        prefabLibrary = prefabList.Where(p => p != null).ToDictionary(p => p.name);
    }

    private void Start() => LoadWorld();
    private void OnApplicationQuit() => SaveWorld();

    public void SaveWorld()
    {
        WorldSaveData world = new();
        foreach (var entity in FindObjectsOfType<ChunkableEntity>())
        {
            if (entity.IsDestroyed) continue;

            string prefabName = entity.gameObject.name.Replace("(Clone)", "").Trim();
            world.entities.Add(new SavedEntity
            {
                uniqueID = entity.UniqueID,
                prefabName = prefabName,
                x = entity.transform.position.x,
                y = entity.transform.position.y,
                z = entity.transform.position.z,
                parentName = entity.ParentName
            });
        }
        File.WriteAllText(SavePath, JsonUtility.ToJson(world, true));
    }

    public void LoadWorld()
    {
        var initialEntities = FindObjectsOfType<ChunkableEntity>().ToDictionary(e => e.UniqueID);

        if (!File.Exists(SavePath))
        {
            if (initialEntities.Any())
                ChunkableEntity.nextID = initialEntities.Keys.Max() + 1;
            SaveWorld(); // створюємо JSON при першому запуску
            return;
        }

        string json = File.ReadAllText(SavePath);
        WorldSaveData world = JsonUtility.FromJson<WorldSaveData>(json);

        int maxID = world.entities.Any() ? world.entities.Max(e => e.uniqueID) : 0;
        int maxInitialID = initialEntities.Any() ? initialEntities.Keys.Max() : 0;
        ChunkableEntity.nextID = Mathf.Max(maxID, maxInitialID) + 1;

        HashSet<int> loadedIDs = world.entities.Select(e => e.uniqueID).ToHashSet();

        // видаляємо об’єкти, яких більше немає у JSON
        foreach (var entity in initialEntities.Values)
            if (!loadedIDs.Contains(entity.UniqueID)) Destroy(entity.gameObject);

        // Інстанціюємо нові об’єкти
        foreach (var data in world.entities)
        {
            if (!prefabLibrary.ContainsKey(data.prefabName)) continue;

            if (initialEntities.TryGetValue(data.uniqueID, out ChunkableEntity exist))
            {
                exist.transform.position = new(data.x, data.y, data.z);
                continue;
            }

            GameObject obj = Instantiate(prefabLibrary[data.prefabName], new Vector3(data.x, data.y, data.z), Quaternion.identity);
            obj.GetComponent<ChunkableEntity>().UniqueID = data.uniqueID;

            // встановлюємо батька
            if (!string.IsNullOrEmpty(data.parentName))
            {
                GameObject parentObj = GameObject.Find(data.parentName);
                if (parentObj != null)
                    obj.transform.parent = parentObj.transform;
            }

            // реєструємо у чанках
            Vector2Int chunkID = ChunkHandler.Instance.GetChunkID(obj.transform.position);
            ChunkHandler.Instance.RegisterEntity(obj);
        }
    }

    [System.Serializable]
    public class SavedEntity
    {
        public int uniqueID;
        public string prefabName;
        public float x, y, z;
        public string parentName;
    }

    [System.Serializable]
    public class WorldSaveData
    {
        public List<SavedEntity> entities = new();
    }
}
