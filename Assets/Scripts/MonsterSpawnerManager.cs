using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawnerManager : MonoBehaviour
{
    // public SpawnerMonster spawnerMonster;
    public List<SpawnerMonster> spawnersList = new List<SpawnerMonster>();

    public GameObject currentSpawner;
    void Start()
    {
        if (spawnersList.Count < 1)
        {
            Debug.LogError("La liste des spawners doit contenir au moins un élément !");
            return;
        }
        SpawnNextMonster();
    }

    public void SpawnNextMonster()
    {
        // Destroy previous objects if they exist
        if (currentSpawner != null)
        {
            Destroy(currentSpawner);
            currentSpawner = null;
        }

        // Select a random spawner
        int spawnerIndex = Random.Range(0, spawnersList.Count);
        SpawnerMonster selectedSpawner = spawnersList[spawnerIndex];

        // Spawn the monster at the selected spawner
        currentSpawner = selectedSpawner.SpawnMonster();
    }
    

}