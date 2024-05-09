using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrain : MonoBehaviour
{
    public GameObject train;
    public float spawnInterval = 60.0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) {
            yield return new WaitForSeconds(spawnInterval); 
            createTrain();
        }
    }

    private void createTrain()
    {
        Instantiate(train, transform.position, transform.rotation); // Instantiate the object at the spawner's position and rotation
    }
}
