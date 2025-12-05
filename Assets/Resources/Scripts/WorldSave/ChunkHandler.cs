using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ћог≥ка чанк≥в
public class ChunkHandler : MonoBehaviour
{
    public static ChunkHandler Instance { get; private set; }

    [SerializeField] private GameObject player;
    [SerializeField] private int CHUNK_SIZE = 16;
    [SerializeField] private int LOAD_RADIUS_CHUNKS = 1;
    [SerializeField] private float checkInterval = 1f;

    private Dictionary<Vector2Int, List<GameObject>> chunkedEntities = new();
    private HashSet<Vector2Int> activeChunks = new();
    private Dictionary<Vector2Int, Transform> chunkParents = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!player) { Debug.LogError("Player not set!"); return; }
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return null;
        StartCoroutine(CheckChunks());
        ActivateAround(GetChunkID(player.transform.position));
    }

    public void RegisterEntity(GameObject obj)
    {
        if (!obj) return;

        var entity = obj.GetComponent<ChunkableEntity>();

        Vector2Int id = GetChunkID(obj.transform.position);
        if (!chunkedEntities.ContainsKey(id)) chunkedEntities[id] = new List<GameObject>();
        chunkedEntities[id].Add(obj);
    }

    public void UnregisterEntity(GameObject obj)
    {
        if (!obj) return;
        Vector2Int id = GetChunkID(obj.transform.position);
        chunkedEntities.GetValueOrDefault(id)?.Remove(obj);
    }

    public Vector2Int GetChunkID(Vector3 pos) =>
        new(Mathf.FloorToInt(pos.x / CHUNK_SIZE), Mathf.FloorToInt(pos.y / CHUNK_SIZE));

    public Transform GetChunkParent(Vector2Int id)
    {
        if (!chunkParents.ContainsKey(id))
        {
            GameObject go = new GameObject($"Chunk_{id.x}_{id.y}");
            chunkParents[id] = go.transform;
        }
        return chunkParents[id];
    }

    private void ActivateAround(Vector2Int center)
    {
        for (int x = -LOAD_RADIUS_CHUNKS; x <= LOAD_RADIUS_CHUNKS; x++)
            for (int y = -LOAD_RADIUS_CHUNKS; y <= LOAD_RADIUS_CHUNKS; y++)
            {
                Vector2Int id = new(center.x + x, center.y + y);
                if (activeChunks.Add(id)) SetEntitiesActive(id, true);
            }
    }

    private void SetEntitiesActive(Vector2Int id, bool active)
    {
        if (!chunkedEntities.ContainsKey(id)) return;
        var list = chunkedEntities[id];
        for (int i = list.Count - 1; i >= 0; i--)
            if (list[i]) list[i].SetActive(active);
            else list.RemoveAt(i);
    }

    private IEnumerator CheckChunks()
    {
        while (true)
        {
            Vector2Int pChunk = GetChunkID(player.transform.position);
            HashSet<Vector2Int> needed = new();

            for (int x = -LOAD_RADIUS_CHUNKS; x <= LOAD_RADIUS_CHUNKS; x++)
                for (int y = -LOAD_RADIUS_CHUNKS; y <= LOAD_RADIUS_CHUNKS; y++)
                    needed.Add(new(pChunk.x + x, pChunk.y + y));

            foreach (var id in new List<Vector2Int>(activeChunks))
                if (!needed.Contains(id)) { SetEntitiesActive(id, false); activeChunks.Remove(id); }

            foreach (var id in needed)
                if (activeChunks.Add(id)) SetEntitiesActive(id, true);

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
