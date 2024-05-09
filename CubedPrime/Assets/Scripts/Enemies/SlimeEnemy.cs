using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public int slimeSpawns = 3;
    private Enemy enemy;
    
    public GameObject slimes;
    public GameObject onDeathEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.getHealth() <= 0)
        {
            DivideEnemy();
        }
    }

    private void DivideEnemy()
    {
        for (int i = 0; i < slimeSpawns; i++)
        {
            Vector3 offset = Random.insideUnitCircle * 3.5f; 
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);
            PlayerStats.instance.currentEnemiesCount++;
            Instantiate(slimes, spawnPos, Quaternion.identity);
        }

        if (onDeathEffect != null)
        {
            Instantiate(onDeathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
