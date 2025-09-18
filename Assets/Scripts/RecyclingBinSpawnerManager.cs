using UnityEngine;
using System.Collections.Generic;

public class RecyclingBinSpawnerManager : MonoBehaviour
{
    public List<SpawnerObject> binSpawnerList = new List<SpawnerObject>();

    void Start()
    {
        if (binSpawnerList == null)
        {
            Debug.LogError("La liste des spawners est null!");
            return;
        }
        SpawnUniqueBins();
    }
    // Call this to spawn unique bins on each spawner
    public void SpawnUniqueBins()
    {
        string[] types = { "Plastic", "Metal", "Paper", "Glass" };
        List<string> availableTypes = new List<string>(types);
        System.Random rng = new System.Random();

        for (int i = 0; i < binSpawnerList.Count && availableTypes.Count > 0; i++)
        {
            // Pick a random type from availableTypes
            int typeIdx = rng.Next(availableTypes.Count);
            string chosenType = availableTypes[typeIdx];
            availableTypes.RemoveAt(typeIdx);

            // Always spawn bin at index 0
            binSpawnerList[i].SpawnObjectAndReturn(chosenType, 0);
        }
    }
}