using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : MonoBehaviour
{
    
    public GameObject explosion;

    private Enemy enemy;
    private bool exploded;
    private ExplosionObj explosionStats;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        explosionStats = explosion.GetComponent<ExplosionObj>();
        explosionStats.target_tags = "all";
    }

    private void Update()
    {
        if (enemy.health <= 0 && !exploded)
        {
            DeathExplosion();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !exploded)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        exploded = true;
        Destroy(gameObject);
    }

    private void DeathExplosion()
    {
        exploded = true;
        Instantiate(explosion, transform.position, Quaternion.identity);
        enemy.EnemyDeath();
    }

   
}
