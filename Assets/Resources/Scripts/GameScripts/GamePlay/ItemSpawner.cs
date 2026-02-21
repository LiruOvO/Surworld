using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public Tilemap targetTilemap;

    [Header("Item To Spawn")]
    public CollectableType itemType;

    [Header("Settings")]
    public int maxItems = 10;
    public float checkInterval = 10f;
    public float initialDelay = 2.0f; // Збільшено затримку для безпеки
    public Transform itemsParent;

    private List<Vector3Int> allAvailableTiles = new List<Vector3Int>();

    void Start()
    {
        if (itemsParent == null) itemsParent = this.transform;

        GetAllTiles();

        // Важливо: використовуємо більшу затримку при старті
        InvokeRepeating(nameof(MaintainItemCount), initialDelay, checkInterval);
    }

    void MaintainItemCount()
    {
        if (ItemManager.Instance == null) return;

        // КРОК 1: Шукаємо ВЗАГАЛІ ВСІ предмети цього типу на сцені
        // Це допоможе, навіть якщо система збереження ще не поклала їх у папку
        Collectable[] allCollectables = Object.FindObjectsByType<Collectable>(FindObjectsSortMode.None);

        int currentCount = 0;
        foreach (var item in allCollectables)
        {
            if (item.type == itemType)
            {
                currentCount++;

                // КРОК 2: Якщо предмет знайдений на сцені, але він не в нашій папці — 
                // примусово переміщуємо його туди (якщо він не має батька)
                if (item.transform.parent == null || item.transform.parent.name == "Collectables")
                {
                    item.transform.SetParent(itemsParent);
                }
            }
        }

        Debug.Log($"[Spawner {itemType}]: Перевірка... Знайдено всього: {currentCount}. Ліміт: {maxItems}");

        // КРОК 3: Спавнимо лише якщо реально не вистачає до ліміту
        if (currentCount < maxItems)
        {
            int toSpawn = maxItems - currentCount;
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnSingleItem();
            }
        }
    }

    void SpawnSingleItem()
    {
        if (allAvailableTiles.Count == 0) return;

        Collectable prefab = ItemManager.Instance.GetItemByType(itemType);
        if (prefab == null) return;

        Vector3Int randomTile = allAvailableTiles[Random.Range(0, allAvailableTiles.Count)];
        Vector3 spawnPos = targetTilemap.GetCellCenterWorld(randomTile);

        // Перевірка на Overlap, щоб не класти палиці одну на одну
        if (Physics2D.OverlapCircle(spawnPos, 0.2f) == null)
        {
            GameObject newItem = Instantiate(prefab.gameObject, spawnPos, Quaternion.identity);
            newItem.transform.SetParent(itemsParent);
            newItem.name = prefab.name;
        }
    }

    void GetAllTiles()
    {
        allAvailableTiles.Clear();
        if (targetTilemap == null) return;

        BoundsInt bounds = targetTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (targetTilemap.HasTile(pos)) allAvailableTiles.Add(pos);
        }
    }
}