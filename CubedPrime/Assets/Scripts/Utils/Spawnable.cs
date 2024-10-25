using UnityEngine;

[System.Serializable]
public class Spawnable {
    [Tooltip("The item which could be spawned.")]
    public GameObject obj;
    
    [Tooltip("Chance to spawn an item. which is a weight out of 0.0 to 1")]
    [Range(0.0f,1.0f)]
    public float spawnChance; 
}