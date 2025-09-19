using UnityEngine;
using System.Collections.Generic;

public class RecyclableSpawnerManager : MonoBehaviour
{
    public List<SpawnerObject> spawnersList = new List<SpawnerObject>();

    private int lastObjectSpawnerIndex = -1;
    private int lastBinSpawnerIndex = -1;
    public GameObject currentRecyclableObject;
    public GameObject currentBinObject;
    public GameObject firstBinObject;
    public GameObject secondBinObject;

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
        Destroy(firstBinObject);
        firstBinObject = null;
        Destroy(secondBinObject);
        secondBinObject = null;

        // Randomly select two different bin types
        string[] types = { "Plastic", "Paper", "Glass", "Metal" };
        List<string> availableTypes = new List<string>(types);
        int firstTypeIndex = Random.Range(0, availableTypes.Count);
        string firstBinType = availableTypes[firstTypeIndex];
        availableTypes.RemoveAt(firstTypeIndex);
        int secondTypeIndex = Random.Range(0, availableTypes.Count);
        string secondBinType = availableTypes[secondTypeIndex];

        // Select three different spawners
        int firstBinSpawnerIndex, secondBinSpawnerIndex, objectSpawnerIndex;
        do
        {
            firstBinSpawnerIndex = Random.Range(0, spawnersList.Count);
        } while (spawnersList.Count > 1 && firstBinSpawnerIndex == lastBinSpawnerIndex);
        lastBinSpawnerIndex = firstBinSpawnerIndex;

        do
        {
            secondBinSpawnerIndex = Random.Range(0, spawnersList.Count);
        } while (secondBinSpawnerIndex == firstBinSpawnerIndex && spawnersList.Count > 1);

        do
        {
            objectSpawnerIndex = Random.Range(0, spawnersList.Count);
        } while ((objectSpawnerIndex == firstBinSpawnerIndex || objectSpawnerIndex == secondBinSpawnerIndex) && spawnersList.Count > 2);

        SpawnerObject firstBinSpawner = spawnersList[firstBinSpawnerIndex];
        SpawnerObject secondBinSpawner = spawnersList[secondBinSpawnerIndex];
        SpawnerObject objectSpawner = spawnersList[objectSpawnerIndex];

        // Get prefab lists for each bin type
        List<GameObject> firstBinList = GetPrefabList(firstBinSpawner, firstBinType);
        List<GameObject> secondBinList = GetPrefabList(secondBinSpawner, secondBinType);

        if (firstBinList == null || firstBinList.Count == 0)
        {
            Debug.LogWarning($"No bins for type {firstBinType} in spawner {firstBinSpawnerIndex}");
            return;
        }
        if (secondBinList == null || secondBinList.Count == 0)
        {
            Debug.LogWarning($"No bins for type {secondBinType} in spawner {secondBinSpawnerIndex}");
            return;
        }

        // Always use index 0 for bin
        firstBinObject = firstBinSpawner.SpawnObjectAndReturn(firstBinType, 0);
        secondBinObject = secondBinSpawner.SpawnObjectAndReturn(secondBinType, 0);

        // Randomly choose one of the bin types for the recyclable object
        string objectType = (Random.value < 0.5f) ? firstBinType : secondBinType;
        List<GameObject> objectList = GetPrefabList(objectSpawner, objectType);
        if (objectList == null || objectList.Count <= 1)
        {
            Debug.LogWarning($"No recyclable object for type {objectType} in spawner {objectSpawnerIndex}");
            return;
        }
        currentRecyclableObject = objectSpawner.SpawnObjectAndReturn(objectType, 1);
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