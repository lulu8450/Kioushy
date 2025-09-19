using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerMonster : MonoBehaviour
{
    [Header("Monster Prefab")]
    public GameObject monsterPrefab;

    [Header("Plastic Prefabs")]
    public List<GameObject> PlasticPrefabs = new List<GameObject>();
    [Header("Paper Prefabs")]
    public List<GameObject> PaperPrefabs = new List<GameObject>();
    [Header("Glass Prefabs")]
    public List<GameObject> GlassPrefabs = new List<GameObject>();
    [Header("Metal Prefabs")]
    public List<GameObject> MetalPrefabs = new List<GameObject>();

    // Make Function to spawn monster
    public GameObject SpawnMonster()
    {
        if (monsterPrefab == null)
        {
            Debug.LogWarning("Monster prefab is not assigned!");
            return null;
        }

        return Instantiate(monsterPrefab, transform.position, transform.rotation);
    }


    //TODO: Function to auto spawn random monster on a random spawner, make the monster drop random type of recycling Object 
    //TODO: Create the Monster Prefab, reference it in the inspector, and implement the spawning logic
    //TODO: Remake some Sprite for Recycling Objects
    //TODO: Implement the logic for the monster to drop a random recycling object on death


    public void DropRandomRecyclingObject()
    {
        // Example logic to drop a random recycling object
        string[] types = { "Plastic", "Paper", "Glass", "Metal" };
        System.Random rng = new System.Random();
        string randomType = types[rng.Next(types.Length)];

        List<GameObject> selectedList = null;
        switch (randomType.ToLower())
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
        }

        if (selectedList != null && selectedList.Count > 0)
        {
            int randomIndex = rng.Next(selectedList.Count);
            GameObject dropPrefab = selectedList[randomIndex];
            if (dropPrefab != null)
            {
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            }
        }
    }

}
