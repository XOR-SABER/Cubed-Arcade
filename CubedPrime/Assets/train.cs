using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class train : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f; 
    public float destroyDelay = 0.5f; 
    public float radius = 100.0f; // Radius of the sphere
    public float maxDistance = 0.0f; // Max distance the sphere cast will check
    public LayerMask playerLayer;
    private bool _player_heard = false;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.position) < 0.001f) Invoke("DestroyGameObject", destroyDelay);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.right, maxDistance, playerLayer);
        if (hit.collider != null && !_player_heard)
        {
            _player_heard = true;
            AudioManager.instance.PlayOnShot("TrainHorn");
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject); // Destroy the object
    }
}
