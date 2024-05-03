using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    // Bullets
    public bool isMissile;
    
    // Stats
    public float speed = 10f;
    public int damage = 1;
    public float bulletLifeTime = 5f;
    
    // Player
    private GameObject player;
    private Transform playerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.GetComponent<Transform>();
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            if (isMissile)
            {
                transform.LookAt(playerTransform);
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                transform.position += direction * (speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * (speed * Time.deltaTime));
                Destroy(gameObject, bulletLifeTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerStats.instance.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
