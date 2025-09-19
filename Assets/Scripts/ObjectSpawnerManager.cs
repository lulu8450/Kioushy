using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawnerManager : MonoBehaviour
{

    public List<SpawnerObject> spawnersList = new List<SpawnerObject>();

    public GameObject currentSpawner;
    void Start()
    {
        if (spawnersList.Count < 1)
        {
            Debug.LogError("La liste des spawners doit contenir au moins un élément !");
            return;
        }
        SpawnNextObject();
    }

    public void SpawnNextObject()
    {
        // Destroy previous objects if they exist
        if (currentSpawner != null)
        {
            Destroy(currentSpawner);
            currentSpawner = null;
        }

        // Select a random spawner
        int spawnerIndex = Random.Range(0, spawnersList.Count);
        SpawnerObject selectedSpawner = spawnersList[spawnerIndex];

        // Spawn the monster at the selected spawner
        currentSpawner = selectedSpawner.SpawnObject();
    }
    

}