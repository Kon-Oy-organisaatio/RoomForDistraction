using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    private readonly List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    private void Awake()
    {
        spawnPoints.AddRange(Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None));
    }

    public void SpawnItems(List<GameObject> targetItems, List<GameObject> distractionItems)
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("SpawnManager: No spawn points found in scene.");
            return;
        }

        int totalRequested = targetItems.Count + distractionItems.Count;
        if (totalRequested > spawnPoints.Count)
            Debug.LogWarning($"SpawnManager: Requested {totalRequested} items with only {spawnPoints.Count} spawn points.");

        // Shuffle points
        var points = new List<SpawnPoint>(spawnPoints);
        for (int i = 0; i < points.Count; i++)
        {
            int r = Random.Range(i, points.Count);
            (points[i], points[r]) = (points[r], points[i]);
        }

        int spawnIndex = 0;

        // Spawn targets
        foreach (var prefab in targetItems)
        {
            if (spawnIndex >= points.Count) break;
            if (prefab == null) continue;

            SpawnAt(points[spawnIndex], prefab);
            spawnIndex++;
        }

        // Spawn distractions
        foreach (var prefab in distractionItems)
        {
            if (spawnIndex >= points.Count) break;
            if (prefab == null) continue;

            SpawnAt(points[spawnIndex], prefab);
            spawnIndex++;
        }

        Debug.Log($"SpawnManager: Spawned {spawnIndex}/{totalRequested} requested items.");
    }

    private void SpawnAt(SpawnPoint spawnPoint, GameObject prefab)
    {
        if (spawnPoint == null || prefab == null) return;

        Vector3 spawnPos = spawnPoint.transform.position + Vector3.up * 0.01f;
        GameObject instance = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        if (instance.CompareTag("Juomapullo"))
        {
            instance.transform.Rotate(0f, 90f, 0f);
        }

        instance.transform.SetParent(spawnPoint.transform);
    }
}
