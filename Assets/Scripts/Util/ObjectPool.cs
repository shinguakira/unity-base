using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic key-based object pool. Reduces GC pressure from Instantiate/Destroy.
///
/// Usage:
///   // Prewarm at scene start
///   ObjectPool.Instance.Prewarm("Bullet", bulletPrefab, 20);
///
///   // Spawn
///   GameObject obj = ObjectPool.Instance.Get("Bullet", spawnPos, spawnRot);
///
///   // Return instead of Destroy
///   ObjectPool.Instance.Return("Bullet", obj);
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private readonly Dictionary<string, Queue<GameObject>> pools =
        new Dictionary<string, Queue<GameObject>>();
    private readonly Dictionary<string, GameObject> prefabs =
        new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>Pre-instantiate <paramref name="count"/> copies of <paramref name="prefab"/> under key <paramref name="key"/>.</summary>
    public void Prewarm(string key, GameObject prefab, int count)
    {
        prefabs[key] = prefab;
        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pools[key].Enqueue(obj);
        }
    }

    /// <summary>Get a pooled object, or instantiate a new one if the pool is empty.</summary>
    public GameObject Get(string key, Vector3 position, Quaternion rotation)
    {
        if (pools.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            GameObject obj = queue.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        if (prefabs.TryGetValue(key, out var prefab))
            return Instantiate(prefab, position, rotation);

        Debug.LogWarning($"[ObjectPool] No prefab registered for key '{key}'");
        return null;
    }

    /// <summary>Return an object to the pool instead of destroying it.</summary>
    public void Return(string key, GameObject obj)
    {
        obj.SetActive(false);
        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();
        pools[key].Enqueue(obj);
    }
}
