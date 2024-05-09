using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f; 
    public float destroyDelay = 0.5f; 

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.position) < 0.001f) Invoke("DestroyGameObject", destroyDelay);
        
    }

    void DestroyGameObject()
    {
        Destroy(gameObject); // Destroy the object
    }
}
