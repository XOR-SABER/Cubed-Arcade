using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Enemy
{
    private bool _exploded = false;
    public GameObject explosionEnemies;
    public GameObject explosionPlayer;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_exploded)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Instantiate(explosionPlayer, transform.position, Quaternion.identity);

        _exploded = true;
        Destroy(gameObject);
    }


    public override void EnemyDeath()
    {
        Instantiate(explosionEnemies, transform.position, Quaternion.identity);
        base.EnemyDeath();
    }
}
