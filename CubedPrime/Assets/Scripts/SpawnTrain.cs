using System.Collections;
using UnityEngine;

public class SpawnTrain : MonoBehaviour
{
    public GameObject train;
    public float spawnInterval = 60.0f;
    public float offset = 15.0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(offset); 
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
