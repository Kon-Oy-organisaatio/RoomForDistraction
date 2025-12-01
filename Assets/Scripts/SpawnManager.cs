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

        foreach (var prefab in targetItems)
        {
            if (spawnIndex >= points.Count) break;
            SpawnAt(points[spawnIndex], prefab);
            spawnIndex++;
        }

        foreach (var prefab in distractionItems)
        {
            if (spawnIndex >= points.Count) break;
            SpawnAt(points[spawnIndex], prefab);
            spawnIndex++;
        }

        Debug.Log($"SpawnManager: Spawned {spawnIndex}/{totalRequested} requested items.");
    }
    private void SpawnAt(SpawnPoint spawnPoint, GameObject prefab)
    {
    if (spawnPoint == null || prefab == null) return;

    // Calculate spawn position slightly above the spawn point to avoid clipping
    Vector3 spawnPos = spawnPoint.transform.position + Vector3.up * 0.02f;

    // Instantiate with prefab's default rotation
    GameObject instance = Instantiate(prefab, spawnPos, prefab.transform.rotation);

    // Set the instantiated object's parent to the spawn point for better organization in the hierarchy
    instance.transform.SetParent(spawnPoint.transform);
    }


}
