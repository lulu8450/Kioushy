using UnityEngine;
using System.Collections.Generic;

public class RecyclableSpawnerManager : MonoBehaviour
{
    public List<SpawnerObject> spawnersList = new List<SpawnerObject>();

    private int lastObjectSpawnerIndex = -1;
    private int lastBinSpawnerIndex = -1;
    public GameObject currentRecyclableObject;
    public GameObject currentBinObject;

    void Start()
    {
        if (spawnersList.Count < 2)
        {
            Debug.LogError("La liste des spawners doit contenir au moins deux éléments !");
            return;
        }
        SpawnNextRecyclable();
    }

    public void SpawnNextRecyclable()
    {
        // Destroy previous objects if they exist
        if (currentRecyclableObject != null)
        {
            Destroy(currentRecyclableObject);
            currentRecyclableObject = null;
        }
        if (currentBinObject != null)
        {
            Destroy(currentBinObject);
            currentBinObject = null;
        }

        // Randomly select recyclable type
        string[] types = { "Plastic", "Paper", "Glass", "Metal" };
        int typeIndex = Random.Range(0, types.Length);
        string selectedType = types[typeIndex];

        // Select two different spawners
        int objectSpawnerIndex, binSpawnerIndex;
        do
        {
            objectSpawnerIndex = Random.Range(0, spawnersList.Count);
        } while (objectSpawnerIndex == lastObjectSpawnerIndex && spawnersList.Count > 1);
        lastObjectSpawnerIndex = objectSpawnerIndex;

        do
        {
            binSpawnerIndex = Random.Range(0, spawnersList.Count);
        } while ((binSpawnerIndex == objectSpawnerIndex || binSpawnerIndex == lastBinSpawnerIndex) && spawnersList.Count > 2);
        lastBinSpawnerIndex = binSpawnerIndex;

        SpawnerObject objectSpawner = spawnersList[objectSpawnerIndex];
        SpawnerObject binSpawner = spawnersList[binSpawnerIndex];

        // Get prefab list for selected type
        List<GameObject> objectList = GetPrefabList(objectSpawner, selectedType);
        List<GameObject> binList = GetPrefabList(binSpawner, selectedType);

        if (objectList == null || objectList.Count == 0)
        {
            Debug.LogWarning($"No recyclable objects for type {selectedType} in spawner {objectSpawnerIndex}");
            return;
        }
        if (binList == null || binList.Count == 0)
        {
            Debug.LogWarning($"No bins for type {selectedType} in spawner {binSpawnerIndex}");
            return;
        }

        // Always use index 1 for object, index 0 for bin
        int objectIndex = 1;
        int binIndex = 0;
        if (objectList.Count <= objectIndex)
        {
            Debug.LogWarning($"No recyclable object at index 1 for type {selectedType} in spawner {objectSpawnerIndex}");
            return;
        }
        if (binList.Count <= binIndex)
        {
            Debug.LogWarning($"No bin at index 0 for type {selectedType} in spawner {binSpawnerIndex}");
            return;
        }

        // Spawn recyclable object
        currentRecyclableObject = objectSpawner.SpawnObjectAndReturn(selectedType, objectIndex);
        // Spawn bin
        currentBinObject = binSpawner.SpawnObjectAndReturn(selectedType, binIndex);
    }

    // Helper to get prefab list by type name
    private List<GameObject> GetPrefabList(SpawnerObject spawner, string type)
    {
        switch (type.ToLower())
        {
            case "plastic": return spawner.PlasticPrefabs;
            case "paper": return spawner.PaperPrefabs;
            case "glass": return spawner.GlassPrefabs;
            case "metal": return spawner.MetalPrefabs;
            default: return null;
        }
    }

}