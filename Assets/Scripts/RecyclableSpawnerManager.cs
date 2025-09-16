using UnityEngine;
using System.Collections.Generic;

public class RecyclableSpawnerManager : MonoBehaviour
{
    // On change ObjectSpawner par SpawnerObject
    public List<SpawnerObject> spawnersList = new List<SpawnerObject>();

    private int lastSpawnerIndex = -1;
    
    // Ajouté pour garder une référence de l'objet spawné pour le détruire
    public GameObject currentRecyclableObject; 

    void Start()
    {
        if (spawnersList.Count == 0)
        {
            Debug.LogError("La liste des spawners est vide !");
            return;
        }

        SpawnNextRecyclable();
    }

    public void SpawnNextRecyclable()
    {
        // On détruit l'objet précédent s'il existe
        if (currentRecyclableObject != null)
        {
            Destroy(currentRecyclableObject);
            currentRecyclableObject = null;
        }
        
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnersList.Count);
            randomType = 
            randomTypeIndex = Random.Range(0, 4); // 0: Plastic, 1: Paper, 2: Glass, 3: Metal
        } while (randomIndex == lastSpawnerIndex && spawnersList.Count > 1);

        lastSpawnerIndex = randomIndex;
        
        SpawnerObject spawnerToActivate = spawnersList[randomIndex];
        // On n'a plus besoin d'activer/désactiver le GameObject car il est toujours actif
        // On appelle directement sa fonction de spawn
        spawnerToActivate.SpawnObject();
    }
}