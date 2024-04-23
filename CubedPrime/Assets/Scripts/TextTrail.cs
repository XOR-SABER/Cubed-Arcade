using UnityEngine;
using System.Collections;

public class TextTrail : MonoBehaviour
{
    public float delayInSeconds = 2.0f; // Delay before each duplication
    public int duplicationLimit = 3; // Maximum number of duplications
    public Canvas canvas; // Reference to the Canvas

    private int duplicationCount = 0; // Current count of duplications

    void Start()
    {
        StartCoroutine(DuplicateAfterDelay());
    }

    IEnumerator DuplicateAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        duplicationCount++;
        Debug.Log("Creating clone number: " + duplicationCount);
        GameObject clone = Instantiate(gameObject, canvas.transform); // Instantiate clone as child of canvas
        Destroy(clone.GetComponent<TextTrail>());
        if(duplicationCount < duplicationLimit) StartCoroutine(DuplicateAfterDelay());
    }
}
