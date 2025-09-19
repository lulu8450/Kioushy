using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerObject : MonoBehaviour
{
    [Header("Plastic Prefabs")]
    public List<GameObject> PlasticPrefabs = new List<GameObject>();
    [Header("Paper Prefabs")]
    public List<GameObject> PaperPrefabs = new List<GameObject>();
    [Header("Glass Prefabs")]
    public List<GameObject> GlassPrefabs = new List<GameObject>();
    [Header("Metal Prefabs")]
    public List<GameObject> MetalPrefabs = new List<GameObject>();

    // Instantiates and returns the spawned GameObject for the given type and index
    public GameObject SpawnObjectAndReturn(string type, int index)
    {
        List<GameObject> selectedList = null;
        switch (type.ToLower())
        {
            case "plastic":
                selectedList = PlasticPrefabs;
                break;
            case "paper":
                selectedList = PaperPrefabs;
                break;
            case "glass":
                selectedList = GlassPrefabs;
                break;
            case "metal":
                selectedList = MetalPrefabs;
                break;
            default:
                Debug.LogWarning($"Unknown type: {type}");
                return null;
        }

        if (selectedList == null || index < 0 || index >= selectedList.Count)
        {
            Debug.LogWarning($"Invalid index {index} for type {type}");
            return null;
        }

        GameObject spawnPrefab = selectedList[index];
        if (spawnPrefab == null) return null;

        return Instantiate(spawnPrefab, transform.position, transform.rotation);
    }
    public GameObject SpawnObject() // choose randomly 1 type  to spawn 1 object
    {
        string[] types = new string[] { "plastic", "paper", "glass", "metal" };
        string chosenType = types[UnityEngine.Random.Range(0, types.Length)];
        int index = 0;
        switch (chosenType)
        {
            case "plastic":
                index = UnityEngine.Random.Range(0, PlasticPrefabs.Count);
                break;
            case "paper":
                index = UnityEngine.Random.Range(0, PaperPrefabs.Count);
                break;
            case "glass":
                index = UnityEngine.Random.Range(0, GlassPrefabs.Count);
                break;
            case "metal":
                index = UnityEngine.Random.Range(0, MetalPrefabs.Count);
                break;
        }
        return SpawnObjectAndReturn(chosenType, index);
    }
 
}
