using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class divisonEnemy : Enemy {
    public int childSpawns = 3;
    public GameObject child;

    // I could do some other stuff but this works.. 
    private void _spawn_childen() {
        for (int i = 0; i < childSpawns; i++)
        {
            Vector3 offset = Random.insideUnitCircle * 3.5f; 
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);
            PlayerStats.instance.currentEnemiesCount++;
            Instantiate(child, spawnPos, Quaternion.identity);
        }
    }

    public override void EnemyDeath() {
        _spawn_childen();
        if (onDeathEffect != null) Instantiate(onDeathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}