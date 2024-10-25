using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Enemy
{
    private bool _exploded = false;
    public GameObject explosionEnemies;
    public GameObject explosionPlayer;
    private static PlayerMovement _PLRMOVEMENT = null;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_PLRMOVEMENT == null) return;
        if (other.CompareTag("Player") && !_exploded && !_PLRMOVEMENT.isDashing)
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

    public override void init() {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if(_player_trans == null) return;
        if(_PLRMOVEMENT == null) {
            _PLRMOVEMENT = PlayerStats.instance.getPlayerRef();
        }

        // If its still null.. 
        if(_PLRMOVEMENT == null) return;
    }
}
